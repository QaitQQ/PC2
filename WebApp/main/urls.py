from django.urls import path
from . import views
from django.contrib.staticfiles.storage import staticfiles_storage
from django.views.generic.base import RedirectView

urlpatterns = [
    path('', views.index, name="home"),
    path('One/', views.One, name="one"),
    path('favicon.ico', RedirectView.as_view(url=staticfiles_storage.url('/favicon.ico')))

]
