from django.http import HttpResponse
from django.middleware import csrf
from rest_framework import generics
from ItemPrAppApi.models import Item, ItemDescription
from ItemPrAppApi.serializers import *
from ItemPrAppApi.permissions import IsOwerOrReadOnly
from rest_framework.permissions import IsAuthenticated, IsAdminUser
from rest_framework.parsers import JSONParser 
from django.shortcuts import render
from django.http.response import JsonResponse
from rest_framework import status
from django.views.decorators.csrf import requires_csrf_token, csrf_exempt



class ItemCreateView(generics.CreateAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemsSerializer
    permission_classes=[IsAuthenticated]
    
class ItemListView(generics.ListAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemsSerializer
    permission_classes=[IsAuthenticated]
# class ItemsListCreateView(generics.CreateAPIView):
#     serializer_class = ItemsListCreateSerializer
#     permission_classes=[IsAuthenticated]  



    
class ItemDestroy(generics.DestroyAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemsSerializer
    permission_classes=[IsAuthenticated]
    
class ItemCashDictDictView(generics.ListAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemCashDictSerializer
    permission_classes=[IsAuthenticated]
    
class ClearBaseView(generics.DestroyAPIView):
    lookup_fields = ('pk', 'another field')
    queryset = Item.objects.all()
    serializer_class = ClearBaseSerializer
    permission_classes=[IsAuthenticated]
    
@csrf_exempt
def ItemsListCreateView(request):
    if request.method == 'POST':
        request_data = JSONParser().parse(request)
        serializer = ItemsListCreateSerializer(data=request_data)
    if serializer.is_valid():
        serializer.save()
        return JsonResponse({"IDs":serializer.data}, status=status.HTTP_201_CREATED)
    return JsonResponse(serializer.errors, status=status.HTTP_400_BAD_REQUEST)
    