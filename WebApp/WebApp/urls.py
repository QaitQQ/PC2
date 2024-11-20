from django.urls import path, include
from django.contrib.auth.models import User
from rest_framework import routers, serializers, viewsets
from datetime import datetime
from django.contrib import admin, auth
from django.contrib.auth.views import LoginView, LogoutView
from djoser.urls import authtoken
from django.conf import settings
from django.conf.urls.static import static

# Serializers define the API representation.
class UserSerializer(serializers.HyperlinkedModelSerializer):
    class Meta:
        model = User
        fields = ['url', 'username', 'email', 'is_staff']

# ViewSets define the view behavior.
class UserViewSet(viewsets.ModelViewSet):
    queryset = User.objects.all()
    serializer_class = UserSerializer

# Routers provide an easy way of automatically determining the URL conf.
router = routers.DefaultRouter()
router.register(r'users', UserViewSet)

urlpatterns = [
    path('', include('main.urls'), name='home'),
    path('api/v1/', include('Api.urls'), name='base'),
    path('api/pwa/v1/', include('ItemPrAppApi.urls'), name='pwa_base'),
    path('admin/', admin.site.urls),
    path('accounts/', include('django.contrib.auth.urls')),
    path('api/v1/api-auth/', include('rest_framework.urls'), name='rest_framework'),
    path('api/v1/auth/', include('djoser.urls')),
    path('api/v1/auth_token/',authtoken.views.TokenCreateView.as_view()),
    path('posting_accounting/', include('PostingAccounting.urls')),
    path('auto/', include('AutoH.urls'), name='auto'),
		]  
