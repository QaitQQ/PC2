import os
import posixpath
import socket
BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
SECRET_KEY = 'b99f00a6-710a-457e-bce7-0e06ad38e692'
DEBUG = True 
ALLOWED_HOSTS = ['xn--80ach6cd.xn--p1ai','127.0.0.1','localhost', '192.168.8.100']
INSTALLED_APPS = [
    'django.contrib.admin',
    'django.contrib.admindocs',
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',
    'main',
    'ItemPrAppApi',
    'Api',
    'rest_framework',
    'rest_framework.authtoken',
    'djoser',
    'django_extensions',
    'PostingAccounting',
    'AutoH', 
]
LOGIN_REDIRECT_URL = '/'
MIDDLEWARE = [
    'AutoH.middleware.RemoveHeaders',
    'django.middleware.security.SecurityMiddleware',
    'django.contrib.sessions.middleware.SessionMiddleware',
    'django.middleware.common.CommonMiddleware',
    'django.middleware.csrf.CsrfViewMiddleware',
    'django.contrib.auth.middleware.AuthenticationMiddleware',
    'django.contrib.messages.middleware.MessageMiddleware',
    'django.middleware.clickjacking.XFrameOptionsMiddleware',
]
ROOT_URLCONF = 'WebApp.urls'
TEMPLATES = [
    {
        'BACKEND': 'django.template.backends.django.DjangoTemplates',
        'DIRS': [os.path.join(BASE_DIR, 'templates')],
        'APP_DIRS': True,
        'OPTIONS': {
            'context_processors': [
                'django.template.context_processors.debug',
                'django.template.context_processors.request',
                'django.contrib.auth.context_processors.auth',
                'django.contrib.messages.context_processors.messages',
            ],
        },
    },
]
WSGI_APPLICATION = 'WebApp.wsgi.application'
if str(socket.gethostname()) == "DESKTOP-0":
    DATABASES = {
        'default': {
            'ENGINE': 'django.db.backends.sqlite3',
            'NAME': os.path.join(BASE_DIR, 'db.sqlite3'),
        }
    }
else:
    DATABASES = {
       'default': {
           'ENGINE': 'django.db.backends.mysql',
           'NAME': 'u2163474_dj_def',
           'USER': 'u2163474_dj_def',
           'PASSWORD': 'fn3-XJd-9YF-vBv',
           'HOST': 'localhost',
           'PORT': '3306',
       }
    }
REST_FRAMEWORK = {
    'DEFAULT_PAGINATION_CLASS': 'rest_framework.pagination.LimitOffsetPagination',
    'PAGE_SIZE': 1000,
    'DEFAULT_AUTHENTICATION_CLASSES': [
        'rest_framework.authentication.TokenAuthentication',
        'rest_framework.authentication.BasicAuthentication',
        'rest_framework.authentication.SessionAuthentication',
        ],
    'DEFAULT_PERMISSION_CLASSES': [],
    'DEFAULT_PERMISSION_CLASSES': [
        'rest_framework.permissions.DjangoModelPermissionsOrAnonReadOnly'
    ],
    'DEFAULT_RENDERER_CLASSES': [
        'rest_framework.renderers.JSONRenderer',
        'rest_framework.renderers.BrowsableAPIRenderer',
    ],
    'DEFAULT_METADATA_CLASS': 'rest_framework.metadata.SimpleMetadata'
}
AUTH_PASSWORD_VALIDATORS = [
    {
        'NAME': 'django.contrib.auth.password_validation.UserAttributeSimilarityValidator',
    },
    {
        'NAME': 'django.contrib.auth.password_validation.MinimumLengthValidator',
    },
    {
        'NAME': 'django.contrib.auth.password_validation.CommonPasswordValidator',
    },
    {
        'NAME': 'django.contrib.auth.password_validation.NumericPasswordValidator',
    },
]
LANGUAGE_CODE ='ru'
TIME_ZONE = 'Etc/GMT-3'
USE_I18N = True
USE_L10N = True
USE_TZ = True
STATIC_PRODUCTION_DIR = os.path.abspath(os.path.join(
os.path.dirname(__file__), '..', '..', 'static_production'))
STATIC_URL = 'static/'
STATIC_ROOT = os.path.join(STATIC_PRODUCTION_DIR, "static/")
MEDIA_URL = 'media/'
MEDIA_ROOT = os.path.join(STATIC_PRODUCTION_DIR, "media")
STATICFILES_DIRS = [
   os.path.join(BASE_DIR, "static"),
]
DEFAULT_AUTO_FIELD = 'django.db.models.BigAutoField'