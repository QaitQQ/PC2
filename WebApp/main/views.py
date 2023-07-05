from django.shortcuts import render

from main.models import Atable


def index(request):

    data = {'title':'Переданные данные', 'values':['один', 1, 2.4] }


    return render(request, 'main/index.html', data)



def One(request):
    tables = Atable.objects.order_by("-date")[:2]
    return render(request, 'main/One.html',{'tables':tables})


def add(request):
    return render(request, 'main/add.html')