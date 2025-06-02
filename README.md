# OficinasMecanicas
Projeto Front and Back 
    Front: Web MCV
    Back:  Web API

Estrutura banco:
  Backup: OficinasMecanica.Bak
  Scritp Criação e insert: OficinasMecanicas_Crreate_Inserts.sql
  Script Migrations no projeto: OficinasMecanicas.Dados/Migrations
  
Configurações
    appsettings.json
     Chave do conexção com databaase:  ConnectionStrings=> "DefaultConnection";
     Chave API segurança JWT:  "jwt" =>  "secretKey" , "issuer" , "audience".
     Endereço base da Web API : "baseURL" =>  "link"
     
Setup:
  Realize a restauraçaõ da base de OficinasMecanica.Bak no SQL Server.
  Execução do Web API:
    Realize o Deploy no IIS,e modifiquei o appsettings.json no projeto Front WEB,
	na Chave "baseURL" => "link" como endereço de publicação do Web API ou 
	o comando de dotnet run --urls="https://localhost:7174;http://localhost:5213" WebServicoAPI
	no diretorio do Projeto do Web API.
  	
	