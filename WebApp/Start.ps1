cd D:\YandexDisk\Apps\PC2_solution\WebApp\
.\env\Scripts\activate
$HostIP = (Get-NetIPConfiguration |Where-Object {$_.IPv4DefaultGateway -ne $null -and$_.NetAdapter.Status -ne "Disconnected"}).IPv4Address.IPAddress 
$HostIP = $HostIP+':65529';
python manage.py runserver_plus $HostIP --cert-file cert.pem --key-file key.pem