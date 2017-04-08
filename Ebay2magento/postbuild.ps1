cd CallbackServer
dotnet restore
dotnet publish -c Release -r win10-x86
#cd ../Ebay2magento/bin/x86/Debug
#mdkir CallbackServer
#cd CallbackServer
#robocopy '..\..\..\..\..\CallbackServer\bin\Release\netcoreapp1.1\win10-x86\publish' '.' /E