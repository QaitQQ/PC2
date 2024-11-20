from django.urls import path
from AutoH import views
from django.contrib.staticfiles.storage import staticfiles_storage
from django.views.generic.base import RedirectView



urlpatterns = [
    path('', views.index, name="autoHmainPage"),
    path('devices', views.devices, name="devices"),
    path('data_ex', views.data_ex, name="data_ex"),
    path('rem_dev/<int:pk>', views.rem_dev, name="rem_dev"),
    path('rem_opt', views.rem_opt, name="rem_opt"),
    path('on_off_relay', views.on_off_relay, name="on_off_relay"),
    path('add_opt', views.add_opt, name="add_opt"),
]