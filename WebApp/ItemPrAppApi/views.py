from django.http import HttpResponse
from rest_framework import generics
from ItemPrAppApi.models import Item, ItemDescription
from ItemPrAppApi.serializers import *
from ItemPrAppApi.permissions import IsOwerOrReadOnly
from rest_framework.permissions import IsAuthenticated, IsAdminUser





class ItemCreateView(generics.CreateAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemsSerializer
    permission_classes=[IsAuthenticated]
    
class ItemListView(generics.ListAPIView):
    queryset = Item.objects.all()
    serializer_class = ItemsSerializer
    permission_classes=[IsAuthenticated]
class ItemsListCreateView(generics.CreateAPIView):
   # queryset = list(Item.objects.all())
    serializer_class = ItemsListCreateSerializer
    permission_classes=[IsAuthenticated]    
    
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