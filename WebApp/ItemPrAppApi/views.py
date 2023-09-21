from django.http import HttpResponse
from django.middleware import csrf
from rest_framework import generics
from rest_framework.views import Response
from ItemPrAppApi.models import Item, ItemDescription
from ItemPrAppApi.serializers import *
from ItemPrAppApi.permissions import IsOwerOrReadOnly
from rest_framework.permissions import IsAuthenticated, IsAdminUser, AllowAny
from rest_framework.parsers import JSONParser 
from django.shortcuts import render
from django.http.response import JsonResponse
from rest_framework import status
from django.core import serializers
from django.views.decorators.csrf import requires_csrf_token, csrf_exempt
from rest_framework.decorators import api_view, permission_classes
@csrf_exempt
@api_view(['GET'])
@permission_classes([AllowAny])
def ItemGetView(request):
    if request.method == 'GET':


        import re
        regex = re.compile('^HTTP_')
        print(str( dict((regex.sub('', header), value) for (header, value) 
        in request.META.items() if header.startswith('HTTP_'))))


        item_id = request.GET.get('item_id', '')
        item = Item.objects.filter(id =item_id)
        data =ItemsSerializer(item, many=True).data
        return JsonResponse({"Item":data},status=status.HTTP_201_CREATED, safe=False) 
    
class ItemCreateView(generics.CreateAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemsSerializer
    permission_classes=[IsAuthenticated]
   
class ItemListView(generics.ListAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemsSerializer
    permission_classes=[IsAuthenticated]
class ItemDestroy(generics.DestroyAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemsSerializer
    permission_classes=[IsAuthenticated]
@csrf_exempt
@api_view(['POST'])
@permission_classes([IsAuthenticated])
def ItemSearchView(request):
    
    import re
    regex = re.compile('^HTTP_')
    print(str( dict((regex.sub('', header), value) for (header, value) 
    in request.META.items() if header.startswith('HTTP_'))))

    if request.method == 'POST':
       request_data = JSONParser().parse(request)
       name_booble = request_data.pop("cname")
       itemsName = ComparisonName.objects.filter(cname__contains=name_booble)
       data =ComparisonNameSerializer(itemsName, many=True).data
       return JsonResponse({"Items":data},status=status.HTTP_201_CREATED, safe=False) 

class ItemCashDictDictView(generics.ListAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemCashDictSerializer
    permission_classes=[IsAuthenticated]
@csrf_exempt
@api_view(['DELETE'])
@permission_classes([IsAuthenticated])
def ClearBaseView(request):
    Item.objects.all().delete()
    return JsonResponse({"Result":"success"}, status=status.HTTP_201_CREATED)
@csrf_exempt
@api_view(['POST'])
@permission_classes([IsAuthenticated])
def ItemsListCreateView(request):
    if request.method == 'POST':
        request_data = JSONParser().parse(request)
        serializer = ItemsListCreateSerializer(data=request_data)
    if serializer.is_valid():
        serializer.save()
        return JsonResponse({"IDs":serializer.get_ids()}, status=status.HTTP_201_CREATED)
    return JsonResponse(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
