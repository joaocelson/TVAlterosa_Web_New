-- Geração de Modelo físico
-- Sql ANSI 2003 - brModelo.
CREATE DATABASE CAMPEONATOLD;



CREATE TABLE usuario (
id int PRIMARY KEY identity(1,1) ,
nome_usuario nVarchar(100),
senha_usuario nVarchar(12),
email_usuario nVarchar(100),
tipo_usuario int
)

CREATE TABLE t_bolao (
id int PRIMARY KEY identity(1,1),
gol_time_mandante int,
gol_time_visitante int,
pontos_adquiridos int,
id_partida int,
id_usuario int,
FOREIGN KEY(id_usuario) REFERENCES usuario (id)
)


CREATE TABLE acessos (
id int PRIMARY KEY identity(1,1),
numero_acessos int ,
)

CREATE TABLE time_campeonato (
id int PRIMARY KEY identity(1,1),
id_time int,
id_campeonato int
)

CREATE TABLE classificacao (
id int PRIMARY KEY identity(1,1),
pontos int,
jogos int,
vitoria int,
derrota int,
empate int,
gol_pro int,
gol_contra int,
id_campeonato int,
id_time int
)

CREATE TABLE t_jogador (
id int PRIMARY KEY identity(1,1),
nome nVarchar(100),
foto nVarchar(100),
posicao nVarchar(100),
descricao nVarchar(100)
)

CREATE TABLE times (
id int PRIMARY KEY identity(1,1),
nome nVarchar(100),
escudo nVarchar(100),
presidente nVarchar(100),
descricao nVarchar(500),
telefone nVarchar(100),
data_fundacao DateTime
)

select * from campeonato
CREATE TABLE campeonato (
id int PRIMARY KEY identity(1,1),
data_inicio datetime,
id_bolao int,
nome nVarchar(100),
FOREIGN KEY(id_bolao) REFERENCES t_bolao_campeonato (id)
)

alter table campeonato 
add  id_bolao int
FOREIGN KEY(id_bolao) REFERENCES t_bolao_campeonato (id);

CREATE TABLE partida (
id int PRIMARY KEY identity(1,1),
id_time_mandante int,
id_time_visitante int,
id_campeonato int,
data_partida datetime,
local_partida nVarchar(100),
rodada nVarchar(100),
remarcada_partida bit,
gol_time_mandante int,
gol_time_visitante int,
pontos_computado int,
nova_data_partida DateTime,
FOREIGN KEY(id_time_mandante) REFERENCES times (id),
FOREIGN KEY(id_time_visitante) REFERENCES times (id),
FOREIGN KEY(id_campeonato) REFERENCES campeonato (id)
)

CREATE TABLE T_BOLAO_CAMPEONATO (
id int PRIMARY KEY identity(1,1),
nome nVarchar(100)
)

CREATE TABLE fotos_videos (
id int PRIMARY KEY identity(1,1),
caminho nVarchar(100),
derscricao nVarchar(100),
id_time int,
video nVarchar(100),
id_campeonato int,
id_partida int,
FOREIGN KEY(id_time) REFERENCES times (id),
FOREIGN KEY(id_campeonato) REFERENCES campeonato (id),
FOREIGN KEY(id_partida) REFERENCES partida (id)
)

ALTER TABLE t_bolao ADD FOREIGN KEY(id_partida) REFERENCES partida (id)
ALTER TABLE time_campeonato ADD FOREIGN KEY(id_time) REFERENCES times (id)
ALTER TABLE time_campeonato ADD FOREIGN KEY(id_campeonato) REFERENCES campeonato (id)
ALTER TABLE classificacao ADD FOREIGN KEY(id_campeonato) REFERENCES campeonato (id)
ALTER TABLE classificacao ADD FOREIGN KEY(id_time) REFERENCES times (id)

--INSERT USUARIOS

INSERT INTO usuario (nome_usuario, email_usuario, senha_usuario, tipo_usuario)
            values ('JOAOCELSON', 'joaocelson@gmail.com', 'ibititec', 1);

--INSERT T_BOLAO_CAMPEONATO
INSERT INTO T_BOLAO_CAMPEONATO (nome) VALUES ('1ª Divisão');

--INSERT INTO CAMPEONATO
SELECT * FROM campeonato;
INSERT INTO campeonato (nome, data_inicio)
values ('CHAVE A', '20160702 13:00:00 PM');

INSERT INTO campeonato (nome, data_inicio)
values ('CHAVE B', '20160702 13:00:00 PM');

INSERT INTO campeonato (nome, data_inicio)
values ('CHAVE C', '20160702 13:00:00 PM');

INSERT INTO campeonato (nome, data_inicio)
values ('CHAVE D', '20160702 13:00:00 PM');


--INSERT TIME
SELECT * FROM times;




-- CHAVE A
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Seleção de Dores de Campos', 'DoresCampo.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Verdão de Tabuleiro', 'VerdaoTabuleiro.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Champion de Rio Pomba', 'ChampionRioPomba.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Seleção de Mercês', 'SelecaoMercece.png', '','', '20000704 00:00:00 AM', '');

-- CHAVE B

INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Pérolas Negra do Haiti', 'PerolasHaiti.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Sport de juiz de Fora', 'SportJuizFora.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Seleção de Belmiro Braga', 'SelecaoBelmiroBraga.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Santanense de Santa do Deserto', 'SantanenseSantaDeserto.png', '','', '20000704 00:00:00 AM', '');

-- CHAVE C
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('XV de Novembro de Rio Novo', 'XVNovembroRioNovo.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Aymorés de Ubá', 'AymoresUba.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Tibério de Guarani', 'TiberioGuarani.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Prainha de Rio Novo', 'PrainhaRioNovo.png', '','', '20000704 00:00:00 AM', '');

-- CHAVE D
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Núcleo São João Nepomuceno', 'NúcleoSJoaoNepomuceno.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Aymores de Coronel Pacheco', 'AymoresCoronelPacheco.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Seleção de Descoberto', 'SelecaoDescoberto.png', '','', '20000704 00:00:00 AM', '');
INSERT INTO times (nome, escudo, descricao, presidente, data_fundacao, telefone)
VALUES ('Tupi de Juiz de Fora', 'TupiJuizFora.png', '','', '20000704 00:00:00 AM', '');


1,SAODOMINGOS,3
2,PEROBAS,3
3,SERAB,3
4,SOCIAL,3
5,POCOPEDRA,0
6,POLEMICA,0
7,PIUNA,0
8,ORVALHO,0
8,ORVALHO,0


--INSERT TIME_CAMPEONATO

--GRUPO A
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (1,3);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (1,4);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (1,5);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (1,6);
INSERT INTO time_campeonato (id_campeonato, id_time)

--GRUPO A
VALUES (2,7);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (2,8);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (2,9);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (2,10);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (2,11);

--GRUPO A
VALUES (3,7);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (3,8);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (3,9);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (3,10);
--GRUPO A
VALUES (4,7);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (4,8);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (4,9);
INSERT INTO time_campeonato (id_campeonato, id_time)
VALUES (4,10);


--INSERT PARTIDAS

INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160508 01:00:00 PM',4,7,'AALD','1');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160515 01:00:00 PM',11,8,'MINAS','2');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160522 01:00:00 PM',8,4,'SOCIAL','3');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160529 01:00:00 PM',6,5,'SANTA TEREZINHA','4');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160605 01:00:00 PM',8,5,'CRUZEIRO','5');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160612 01:00:00 PM',3,7,'VILA','6');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160619 01:00:00 PM',4,3,'MINAS','7');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160626 01:00:00 PM',8,7,'SOCIAL','8');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160703 01:00:00 PM',3,8,'VILA','9');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160710 01:00:00 PM',9,5,'SANTA TEREZINHA','10');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160717 01:00:00 PM',8,10,'VILA','11');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160724 01:00:00 PM',4,5,'AALD','12');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160731 01:00:00 PM',9,8,'CRUZEIRO','13');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160807 01:00:00 PM',6,4,'MINAS','14');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160814 01:00:00 PM',6,4,'SANTA TEREZINHA','15');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160821 01:00:00 PM',11,9,'SOCIAL','16');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160828 01:00:00 PM',7,10,'CRUZEIRO','17');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160911 01:00:00 PM',11,3,'AALD','18');

INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160508 03:00:00 PM',6,9,'AALD','1');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160515 03:00:00 PM',5,10,'MINAS','2');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160522 03:00:00 PM',9,3,'SOCIAL','3');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160529 03:00:00 PM',7,11,'SANTA TEREZINHA','4');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160605 03:00:00 PM',4,3,'CRUZEIRO','5');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160612 03:00:00 PM',3,6,'VILA','6');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160619 03:00:00 PM',5,3,'MINAS','7');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160626 03:00:00 PM',9,10,'SOCIAL','8');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160703 03:00:00 PM',10,11,'VILA','9');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160710 03:00:00 PM',7,6,'SANTA TEREZINHA','10');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160717 03:00:00 PM',3,7,'VILA','11');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160724 03:00:00 PM',11,6,'AALD','12');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160731 03:00:00 PM',4,10,'CRUZEIRO','13');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160807 03:00:00 PM',5,11,'MINAS','14');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160814 03:00:00 PM',5,7,'SANTA TEREZINHA','15');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160821 03:00:00 PM',3,10,'SOCIAL','16');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160828 03:00:00 PM',9,4,'CRUZEIRO','17');
INSERT INTO partida (id_campeonato,data_partida, id_time_mandante, id_time_visitante, local_partida, rodada) VALUES (2,'20160911 03:00:00 PM',6,8,'AALD','18');