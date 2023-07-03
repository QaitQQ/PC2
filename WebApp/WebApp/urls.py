"""
Definition of urls for WebApp.
"""

from datetime import datetime
from django.urls import path, include
from django.contrib import admin
from django.contrib.auth.views import LoginView, LogoutView
from app import forms, views



urlpatterns = [
    path('', include('main.urls'), name='home'),
    path('admin/', admin.site.urls),
]
