
from tabnanny import verbose
from django.db import models

class Atable(models.Model):

    title = models.CharField('Название', max_length=50)
    anons = models.CharField('Анонс',max_length=250)
    full_text = models.TextField('Текст')
    date = models.DateTimeField('Дата')

    def __str__(self):
        return self.title

    class Meta:
        verbose_name = 'ТаблицаА'
        verbose_name_plural = 'ТаблицыА'


class Btable(models.Model):

    title = models.CharField('Название', max_length=50)
    anons = models.CharField('Анонс',max_length=250)
    full_text = models.TextField('Текст')
    date = models.DateTimeField('Дата')

    def __str__(self):
        return self.title

    class Meta:
        verbose_name = 'ТаблицаB'
        verbose_name_plural = 'ТаблицыB'