from ast import Import
from django.shortcuts import render
from django.views.generic import DetailView, UpdateView, DeleteView
from PostingAccounting.models import Package, Item 


def index(request):
    if request.method == "GET":
       tables = Package.objects.order_by("-orderDate")  # [:2]
       data = {
            "title": "Посылки", 
            'tables': tables,}
       
       
       return render(request, "PostingAccounting/index.html",data )
    else:
     return render(request, "PostingAccounting/index.html")


class PackageDetailView(DetailView):
    model= Package
    template_name='PostingAccounting/DetailView.html'
    context_object_name='list'
    
