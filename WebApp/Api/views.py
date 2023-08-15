#from http.client import HTTPResponse
from django.http import HttpResponse
from rest_framework import generics
from Api.models import Orders, Items
from Api.serializers import *
from Api.permissions import IsOwerOrReadOnly
from rest_framework.permissions import IsAuthenticated, IsAdminUser
# Create your views here.
#def base(request):
#    if request.method == "GET":  
#        msg =  request.GET['pi']
#        return HttpResponse(msg)
class OrderDestroyView(generics.DestroyAPIView):
    queryset = Orders.objects.all()
    serializer_class = OrderSerializer
class OrderCreateView(generics.CreateAPIView):
    queryset = Orders.objects.all()
    serializer_class = OrderSerializer
class OrderListView(generics.ListAPIView):
    serializer_class = OrderListSerializer
    queryset = Orders.objects.all()
    permission_classes = (IsAuthenticated,)
class OrderDetailView(generics.RetrieveUpdateDestroyAPIView):
    queryset = Orders.objects.all()
    serializer_class = OrderSerializer
    permission_classes = (IsOwerOrReadOnly, IsAdminUser)
class ItemsListView(generics.ListAPIView):
    serializer_class = ItemsSerializer
    queryset = Items.objects.all()
    permission_classes = (IsAuthenticated,)
