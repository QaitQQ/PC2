from dataclasses import fields
from email.policy import default
from pyexpat import model
from rest_framework import serializers
from Api.models import Orders, Items


class ItemsSerializer(serializers.ModelSerializer):

    class Meta:
        model = Items
        fields = ['sku', 'count', 'price']

class OrderListSerializer(serializers.ModelSerializer):
    orderItems = ItemsSerializer(many=True)

    class Meta:
        model = Orders
        fields =  ['orderItems', 'orderDate', 'boxingDate','orderNomber']
#'__all__'
        
        
        

class OrderSerializer(serializers.ModelSerializer):

    user = serializers.HiddenField(default=serializers.CurrentUserDefault())
    orderItems = ItemsSerializer(many=True)

    class Meta:
        model = Orders
        fields = '__all__'

    def create(self, validated_data):
        orderItems = validated_data.pop('orderItems')
        order = Orders.objects.create(**validated_data)
        items = list()
        for orderItem in orderItems:
          item = Items.objects.create(order=order, **orderItem)
          items.append(item)
        order.orderItems.set(items)      
        return order

