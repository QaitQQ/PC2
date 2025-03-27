from datetime import datetime, timezone, timedelta
from django.shortcuts import render
from rest_framework.parsers import JSONParser
from django.http.response import JsonResponse
from rest_framework import status
from django.core.exceptions import ObjectDoesNotExist
import json
import re
from types import SimpleNamespace
from AutoH.models import *
from AutoH.serializers import *
from rest_framework.permissions import AllowAny
from rest_framework import status
from django.views.decorators.csrf import csrf_exempt
from rest_framework.decorators import api_view, permission_classes
from django.urls import reverse
from django.http.response import HttpResponseRedirect
from django.http.request import QueryDict
import requests
def index(request):
    return render(request, "AutoH/index.html")
def devices(request):
    timezone_offset = +3.0  # Pacific Standard Time (UTC−08:00)
    tzinfo = timezone(timedelta(hours=timezone_offset))
    date = datetime.now(tzinfo)
    if request.method == "GET":
        qwery = "SELECT * FROM `AutoH_devicestatus` LEFT JOIN (SELECT `AutoH_devicedata`.`id`, `AutoH_devicedata`.`data`  AS `DeviceData`,`AutoH_devicedata`.`device_id`  FROM `AutoH_devicedata`) p ON (`AutoH_devicestatus`.`device_id` = `p`.`device_id`)"
        device = DeviceStatus.objects.raw(qwery)
        for x in device:
            if x.data < (date - timedelta(minutes=5)):
                x.color = "#fcfaf2"
            else:
                x.color = "#98e38d"
            x.id_str = str(x.device.id)
            if x.DeviceData != None:
             
                cd = x.DeviceData.replace("'", '"')
                if x.device.deviceType.name == "valve":
                    try:
                        mess = json.loads(
                            cd, object_hook=lambda d: SimpleNamespace(**d)
                        )
                        x.on_time = mess.on_time
                        x.DateOfLastOperation = mess.DateOfLastOperation
                    except:
                        pass
                elif x.device.deviceType.name == "relay":
                    try:
                        mess = json.loads(
                            cd, object_hook=lambda d: SimpleNamespace(**d)
                        )
                        x.st_relay = mess.RelayStatus
                    except:
                        pass
                elif x.device.deviceType.name == "ext_dev":
                    try:
                        response = requests.get(x.DeviceData)
                        cont = response.content
                        x.time = cont[0:19]
                        x.term = cont[28:33]
                        x.stat = (
                            (cont[44:60])
                            .decode("unicode-escape")
                            .encode("latin1")
                            .decode("utf-8")
                        )
                        x.link = "https://salessab.su/DHA1810/On.php"
                        x.color = "#98e38d"
                    except:
                        pass
        data = {"device": device}
    return render(request, "AutoH/devices.html", data)
def NewDev(dev_id, tape, requestData):
    timezone_offset = +3.0  # Pacific Standard Time (UTC−08:00)
    tzinfo = timezone(timedelta(hours=timezone_offset))
    if not Device.objects.filter(sn=dev_id).exists():
        if not DeviceType.objects.filter(name=tape).exists():
            deviceType = DeviceType.objects.create(name=tape)
            deviceType.save()
        else:
            deviceType = DeviceType.objects.get(name=tape)
        device = Device.objects.create(sn=dev_id, deviceType=deviceType)
        data = DeviceData.objects.create(device=device, data="")
        devStatus = DeviceStatus.objects.create(
            device=device, data=datetime.now(tzinfo)
        )
        device.save()
        data.save()
    else:
        device = Device.objects.get(sn=dev_id)
        data = DeviceData.objects.get(device=device)
        requestData["update_data"] = datetime.now().strftime("%d.%m.%Y_%H:%M")
        data.data = str(requestData)
        data.save()

@csrf_exempt
@api_view(["GET", "POST"])
@permission_classes([AllowAny])
def data_ex(request):
    timezone_offset = +3.0  # Pacific Standard Time (UTC−08:00)
    tzinfo = timezone(timedelta(hours=timezone_offset))
    if request.method == "GET":
        # читаем id отдаем статус
        dev_id = request.GET.get("dev_id", "")
        DateOfLastOperation = request.GET.get("DateOfLastOperation", "")
        try:
            device = Device.objects.get(sn=dev_id)
        except ObjectDoesNotExist:
            device = None
        if device != None:
            if not DeviceData.objects.filter(device=device).exists():
                dev_dat = DeviceData.objects.create(device=device, data="")
                dev_dat.save()
            else:
                dev_dat = DeviceData.objects.get(device=device)
                if dev_dat.data == "":
                    dev_dat.data = "{'status': '0', 'on_time': []}"
                if DateOfLastOperation != None and DateOfLastOperation != "":
                    cd = dev_dat.data.replace("'", '"')
                    data_obj = json.loads(
                        cd, object_hook=lambda d: SimpleNamespace(**d)
                    )
                    data_obj.DateOfLastOperation = DateOfLastOperation
                    pd = json.dumps(
                        data_obj,
                        ensure_ascii=False,
                        default=get_obj_dict,
                    )
                    pd = pd.replace('"', "'")
                    dev_dat.data = pd
                    dev_dat.save()
            if not DeviceStatus.objects.filter(device=device).exists():
                devStatus = DeviceStatus.objects.create(
                    device=device, data=datetime.now(tzinfo)
                )
            else:
                devStatus = DeviceStatus.objects.get(device=device)
                devStatus.data = datetime.now(tzinfo)
            devStatus.save()
        else:
            create = request.GET.get("create", "")
            tape = request.GET.get("tape", "")
            if create == "new":
                print("создание")
                data = '"status":"0"'
                NewDev(dev_id, tape, [data])
            else:
                return JsonResponse(
                    {"Result": "NoDevice"}, status=status.HTTP_400_BAD_REQUEST
                )
    elif request.method == "POST":
        # # читаем id принимаем статус
        request_data = JSONParser().parse(request)
        dev_id = request_data.pop("dev_id")
        tape = request_data.pop("tape")
        requestData = request_data.pop("data")
  
        try:
            device = Device.objects.get(sn=dev_id)
        except ObjectDoesNotExist:
            device = None
        if device != None:
            data = DeviceData.objects.get(device=device)
            requestData["update_data"] = datetime.now().strftime("%d.%m.%Y_%H:%M")
            data.data = str(requestData)
            data.save()
        else:
            return JsonResponse(
                {"Result": "NoDevice"}, status=status.HTTP_400_BAD_REQUEST
            )
    serData = SingleDataSerializer(
        DeviceData.objects.filter(device=device), many=True
    ).data
    return JsonResponse(
        {"Result": serData, "Time": datetime.now(tzinfo).strftime("%H:%M:%Y_%d:%m")},
        status=status.HTTP_201_CREATED,
        safe=False,
    )
def rem_dev(request, pk):
    dev = Device.objects.get(id=pk)
    DeviceData.objects.filter(device_id=pk).delete()
    DeviceStatus.objects.filter(device_id=pk).delete()
    dev.delete()
    qd = QueryDict(mutable=True)
    return redirect_qd(devices, qd=qd)
@csrf_exempt
@api_view(["POST"])
@permission_classes([AllowAny])
def rem_opt(request):
    mess = request.POST
    rem_val = mess["rem_val"]
    dev_id = mess["dev_id"]
    dev_dat = DeviceData.objects.get(device_id=int(dev_id))
    dev_str = re.sub(r"\'" + rem_val + ".{1,5}',* *", "", dev_dat.data)
    dev_dat.data = dev_str
    dev_dat.save()
    qd = QueryDict(mutable=True)
    qd.update(dev_act=dev_id)
    return redirect_qd(devices, qd=qd)
@csrf_exempt
@api_view(["POST"])
@permission_classes([AllowAny])
def on_off_relay(request):
    mess = request.POST
    dev_id = mess["dev_id"]
    counter = mess["counter"]
    val = mess["val"]
    dev_dat = DeviceData.objects.get(device_id=int(dev_id))
    cd = dev_dat.data.replace("'", '"')
    mess = json.loads(cd, object_hook=lambda d: SimpleNamespace(**d))
    if val == "1":
        mess.RelayStatus[int(counter) - 1] = "0"
    else:
        mess.RelayStatus[int(counter) - 1] = "1"
  
    dev_dat.data = json.dumps(mess.__dict__)
    dev_dat.save()
    qd = QueryDict(mutable=True)
    qd.update(dev_act=dev_id)
    return redirect_qd(devices, qd=qd)
def redirect_qd(viewname, *args, qd=None, **kwargs):
    rev = reverse(viewname, *args, **kwargs)
    if qd:
        rev = "{}?{}".format(rev, qd.urlencode())
    return HttpResponseRedirect(rev)
@csrf_exempt
@api_view(["POST"])
@permission_classes([AllowAny])
def add_opt(request):
    mess = request.POST
    dev_id = mess["dev_id"]
    if mess["hour"] != "" and mess["minutes"] != "" and mess["delay"] != "":
        opt = mess["hour"] + ":" + mess["minutes"] + ":" + mess["delay"] + "   "
        dev_dat = DeviceData.objects.get(device_id=int(dev_id))
        if dev_dat.data == "":
            dev_dat.data = "{'status': '0', 'on_time': []}"
        cd = dev_dat.data.replace("'", '"')
        data = json.loads(cd, object_hook=lambda d: SimpleNamespace(**d))
        data.on_time.append(opt)
        data.update_data = datetime.now().strftime("%d.%m.%Y_%H:%M")
        pd = json.dumps(
            data,
            ensure_ascii=False,
            default=get_obj_dict,
        )
        pd = pd.replace('"', "'")
        # pd = re.sub(r'\"'+rem_val+'.{1,5}\",* *',"",dev_dat.data)
        dev_dat.data = pd
        dev_dat.save()
    qd = QueryDict(mutable=True)
    qd.update(dev_act=dev_id)
    return redirect_qd(devices, qd=qd)
def get_obj_dict(obj):
    return obj.__dict__
def add_new_dev(request):
    mess = request.POST
    name = mess["name"]
    devicedata = mess["devicedata"]
    dev_type = mess["type"]
    timezone_offset = +3.0  # Pacific Standard Time (UTC−08:00)
    tzinfo = timezone(timedelta(hours=timezone_offset))
    if not Device.objects.filter(sn=name).exists():
        if not DeviceType.objects.filter(name=dev_type).exists():
            deviceType = DeviceType.objects.create(name=dev_type)
            deviceType.save()
        else:
            deviceType = DeviceType.objects.get(name=dev_type)
        device = Device.objects.create(sn=name, deviceType=deviceType)
        data = DeviceData.objects.create(device=device, data=devicedata)
        devStatus = DeviceStatus.objects.create(
            device=device, data=datetime.now(tzinfo)
        )
        device.save()
        data.save()
    else:
        device = Device.objects.get(sn=name)
        data = DeviceData.objects.get(device=device)
        data.save()
    qd = QueryDict(mutable=True)
    qd.update(dev_act=device.pk)
    return redirect_qd(devices, qd=qd)
def get_ext_dev(request):
    mess = request.POST
    dev_id = mess["dev_id"] 
    device = Device.objects.get(id=dev_id)
    link = mess["dev_link"]
    print(link)
    response = requests.get(link)
    
    qd = QueryDict(mutable=True)
    qd.update(dev_act=device.pk)
    return redirect_qd(devices, qd=qd) 