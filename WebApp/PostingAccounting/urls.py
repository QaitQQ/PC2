from asyncio.windows_events import NULL
from django.urls import path, include
from PostingAccounting import views
from django.contrib.staticfiles.storage import staticfiles_storage
from django.views.generic.base import RedirectView
from django.contrib.auth.views import LoginView, LogoutView
from django.contrib import admin
from rest_framework import routers, serializers, viewsets
from django.contrib.auth.models import User
from main.views import index






urlpatterns = [
    path('', views.index, name="PostingAccountingMainPage"),
    path('package_detail_view', views.PackageDetailView.as_view(), name="PackageDetailView"),
]
