create table Usuario(
	IdUsuario		integer			Identity(1,1),
	Nome			nvarchar(150)	not null,
	Email			nvarchar(100)	not null unique,
	Senha			nvarchar(50)	not null,
	DataCriacao		datetime		not null,
	primary key(IdUsuario))

create table Compromisso(
	IdCompromisso	integer			Identity(1,1),
	Titulo			nvarchar(150)	not null,
	Descricao		nvarchar(500)	not	null,
	DataInicio		date			not null,
	HoraInicio		time			not null,
	DataFim			date			not null,
	HoraFim			time			not null,
	IdUsuario		integer			not null,
	primary key(IdCompromisso),
	foreign key(IdUsuario) references Usuario(IdUsuario))