from django.contrib import admin
from django.shortcuts import render
from .models import Atable, Btable



admin.site.register(Atable)
admin.site.register(Btable)
# Register your models here.
