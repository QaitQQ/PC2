﻿from django.urls import path
from . import views
from django.contrib.staticfiles.storage import staticfiles_storage
from django.views.generic.base import RedirectView

app_name = 'ItemPrAppApi'

urlpatterns = [

    path('item/create/', views.ItemCreateView.as_view()),
    path('item/list/', views.ItemListView.as_view()),
    path('item/create_list/', views.ItemsListCreateView),
    path('item/item_destroy/<int:pk>', views.ItemDestroy.as_view()),
    path('item/item_cash/', views.ItemCashDictDictView.as_view()),
    path('item/clear_base/', views.ClearBaseView),
    path('item/get_item/', views.ItemGetView),
    path('item/item_search/', views.ItemSearchView)
]
