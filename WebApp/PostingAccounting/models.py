from django.db import models
from datetime import date
from django.contrib.auth import get_user_model

User = get_user_model()

class Package(models.Model):

    orderDate = models.DateTimeField(verbose_name= 'Дата Заказа')
    unboxingDate = models.DateTimeField(verbose_name= 'Дата Распаковки')
    orderNomber = models.CharField("Номер", unique=True, max_length=250)
    orderItems =  models.ManyToOneRel("Items", field_name='orderItems',  on_delete=models.CASCADE, to='order')
    user = models.ForeignKey(User, verbose_name= 'Пользователь', on_delete=models.CASCADE, null=True)
    def get_absolute_url(self):
        return f'/{self.id}'


class Item(models.Model):

    sku = models.CharField(verbose_name="SKU", max_length=50)
    count = models.IntegerField(verbose_name="Количество")
    price = models.DecimalField(verbose_name="Цена", decimal_places=2, max_digits=10)
    order = models.ForeignKey(Package,verbose_name= 'Заказ', related_name= 'PackageItems', on_delete=models.CASCADE, null=True)
    sku = models.CharField(verbose_name="ССЫЛКА", max_length=200)

    def get_absolute_url(self):
        return f'/{self.id}'