from django.urls import path
from . import views
from django.contrib.staticfiles.storage import staticfiles_storage
from django.views.generic.base import RedirectView

app_name = 'Order'

urlpatterns = [
  # path('', views.base, name="base"),
    path('order/create/', views.OrderCreateView.as_view()),
    path('order/all/', views.OrderListView.as_view()),
    path('order/Detail/<int:pk>', views.OrderDetailView.as_view()),
    path('item/all/', views.ItemsListView.as_view()),
    #path('One/', views.One, name="one"),
    #path('add', views.add, name="add"),
    #path('favicon.ico', RedirectView.as_view(url=staticfiles_storage.url('/favicon.ico'))),
    #path( '<int:pk>', views.ListDetailView.as_view(), name="list-detail"),
    #path( '<int:pk>/up', views.DetailUp.as_view(), name="detail-up"),
    #path( '<int:pk>/del', views.DetailDel.as_view(), name="detail-del"),
]
