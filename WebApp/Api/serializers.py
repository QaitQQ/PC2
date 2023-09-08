
from dataclasses import fields
from email.policy import default
from enum import unique
from os import read
from pyexpat import model
import re
from urllib import request
from wsgiref.validate import validator
from rest_framework import serializers
from Api.models import Orders, Items, Store

class ItemsSerializer(serializers.ModelSerializer):
    class Meta:
        model = Items
        fields = ['sku', 'count', 'price']
class StoreSerializer(serializers.ModelSerializer):
    class Meta:
        model = Store
        validators=[]
        fields =  '__all__'

class OrderListSerializer(serializers.ModelSerializer):
    orderItems = ItemsSerializer(many=True)
    store = StoreSerializer(read_only=True)
    class Meta:
        model = Orders
        fields =  ['orderItems', 'orderDate', 'boxingDate','orderNomber', 'store']


class OrderSerializer(serializers.ModelSerializer):
    user = serializers.HiddenField(default=serializers.CurrentUserDefault())
    orderItems = ItemsSerializer(many=True)
    store = StoreSerializer(read_only=True)

    class Meta:
        model = Orders
        fields = '__all__'
    def create(self, validated_data):
        orderItems = validated_data.pop('orderItems')
        storeBooble = self.initial_data.pop('store')     
        if Store.objects.filter(storeApi = storeBooble['storeApi']).exists():
            store = Store.objects.filter(storeApi = storeBooble['storeApi']).first()
        else:
            store = Store.objects.create(**storeBooble)     
        order = Orders.objects.create(**validated_data)
        items = list()
        for orderItem in orderItems:
          item = Items.objects.create(order=order, **orderItem)
          items.append(item)
        order.orderItems.set(items)  
        order.store = store
        order.save()

        return order



