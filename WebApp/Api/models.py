from tabnanny import verbose
from django.db import models
from datetime import date
from django.contrib.auth import get_user_model

User = get_user_model()

class Store(models.Model):

    name = models.CharField(verbose_name="Имя", max_length=50)
    storeApi = models.CharField(verbose_name="ID", unique=True, max_length=50, null=True)
    storeINN = models.CharField(verbose_name="ИНН", max_length=50)

    def get_absolute_url(self):
        return f'/{self.storeApi}'
    def __str__(self):
        return self.id




class Orders(models.Model):

    #дата заказа, дата сборки, номер 

    orderDate = models.DateTimeField(verbose_name= 'Дата Заказа')
    boxingDate = models.DateTimeField(verbose_name= 'Дата Сборки')
    orderNomber = models.CharField("Номер", unique=True, max_length=250)
    orderItems =  models.ManyToOneRel("Items", field_name='orderItems',  on_delete=models.CASCADE, to='order')
    user = models.ForeignKey(User, verbose_name= 'Пользователь', on_delete=models.CASCADE, null=True)
    store = models.ForeignKey(Store, verbose_name='Магазин', on_delete=models.CASCADE, null=True)
    def __str__(self):
        return self.id
    def get_absolute_url(self):
        return f'/{self.id}'

class Items(models.Model):

    sku = models.CharField(verbose_name="SKU", max_length=50)
    count = models.IntegerField(verbose_name="Количество")
    price = models.DecimalField(verbose_name="Цена", decimal_places=2, max_digits=10)
    order = models.ForeignKey(Orders,verbose_name= 'Заказ', related_name= 'orderItems', on_delete=models.CASCADE, null=True)

    def get_absolute_url(self):
        return f'/{self.id}'

