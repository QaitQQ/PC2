from tabnanny import verbose
from django.db import models
from datetime import date
	 
	
class Atable(models.Model):
	title = models.CharField("Название", max_length=50, null=True)
	anons = models.CharField("Анонс", max_length=250, null=True)    
	full_text = models.TextField("Текст", null=True)
	date = models.DateTimeField("Дата", null=date.today())
	def __str__(self):
		return self.title
	def get_absolute_url(self):
		return f'/{self.id}'
class Meta:
	verbose_name = "ТаблицаА"
	verbose_name_plural = "ТаблицыА"
class Btable(models.Model):
	title = models.CharField("Название", max_length=50)
	anons = models.CharField("Анонс", max_length=250)
	full_text = models.TextField("Текст")
	date = models.DateTimeField("Дата")
	def __str__(self):        
		return self.title  


class Meta:
	verbose_name = "ТаблицаB"
	verbose_name_plural = "ТаблицыB"