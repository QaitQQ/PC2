from django.urls import path, include
from . import views
from django.contrib.staticfiles.storage import staticfiles_storage
from django.views.generic.base import RedirectView
from django.contrib.auth.views import LoginView, LogoutView
from django.contrib import admin
from rest_framework import routers, serializers, viewsets
from django.contrib.auth.models import User
from PostingAccounting.views import index

urlpatterns = [
    path('', views.index, name="home"),
    path('One/', views.One, name="one"),
    path('add', views.add, name="add"),
    path('PostingAccounting', index, name="PostingAccounting"),
    path('favicon.ico', RedirectView.as_view(url=staticfiles_storage.url('/favicon.ico'))),
    path( '<int:pk>', views.ListDetailView.as_view(), name="list-detail"),
    path( '<int:pk>/up', views.DetailUp.as_view(), name="detail-up"),
    path( '<int:pk>/del', views.DetailDel.as_view(), name="detail-del"),
]
