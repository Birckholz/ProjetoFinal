INSERT INTO cargos (nomeCargo,salarioBase)
VALUES
    ('Cargo 1',10000.00),
	('Cargo 2',2000.00),
	('Cargo 3',3000.00);

INSERT INTO clientes(nomeCliente,telefoneCliente,emailCliente,enderecoCliente,descricaoCliente,PessFCPFCliente,PessJCNPJCliente,statusCliente)
VALUES
	('Cliente 1','1234-7891','email1.com','BR,PR,Curitiba,Bairro x,Rua y,1',null,null,'12.123.123/0001-12','Em negociação'),
	('Cliente 2','(41) 9 9876-5432','email2.com','BR,PR,Campo Largo,Bairro v,Rua v,1','Cliente em busca de serviço de cibersegurança','123.456.789-10',null,null),
	('Cliente 3','1111-111','email3.com','BR,RJ,Rio de Janeiro,Bairro x,Rua y,1',null,null,'45.567.123/0001-12',null);

--op 2, departamento é opcional, cria um funcionario e depois mando para um departamento, CASO DEPARTAMENTO N EXISTA (caso criado antes, só mandar com cód na criação)
INSERT INTO funcionarios(idCargo,idDepartamento,nomeFuncionario,telefoneFuncionario,emailFuncionario,enderecoFuncionario,CPFFuncionario,tipoContrFuncionario,modoTrabFuncionario,formacaoRelevanteFuncionario,statusFuncionario)
VALUES
	(1,null,'Maria','1234-5678','email4.com','Endereço 1','111.111.111-11','Permanente','Remoto','Mestrado em x','Contratado'),
	(2,null,'Jose','2020-2020','email5.com','Endereço 2','222.222.222-22','Temporário','Híbrido','Técnico em x','Contratado'),
	(3,null,'Marcia','7891-0235','email6.com','Endereço 3','333.333.333-33','Permanente','Presencial',null,'Contratado');

--op 2 , responsavel tem codigo de funcionario, é criado depois de funcionario
INSERT INTO departamentos(nomeDepartamento,idResponsavel)
VALUES
	('Dep 1',1),
	('Dep 2',2),
	('Dep 3',3);

/*--op 1 , responsavel so tem nome, funcionario tem dep obrigatório
INSERT INTO departamentos(nomeDepartamento,responsavelDepartamento)
VALUES
	('Dep 1','Maria'),
	('Dep 2','Jose'),
	('Dep 3','Marcia'),
	('Departamento nao definido','000');--departamento nulo para delete

--op 1, departamento é obrigatório
INSERT INTO funcionarios(idCargo,idDepartamento,nomeFuncionario,telefoneFuncionario,emailFuncionario,enderecoFuncionario,CPFFuncionario,tipoContrFuncionario,modoTrabFuncionario,formacaoRelevanteFuncionario,statusFuncionario)
VALUES
	(1,1,'Maria','1234-5678','email4.com','Endereço 1','111.111.111-11','Permanente','Remoto',null,'Contratado'),
	(2,2,'Jose','2020-2020','email5.com','Endereço 2','222.222.222-22','Temporário','Híbrido',null,'Contratado'),
	(3,3,'Marcia','7891-0235','email6.com','Endereço 3','333.333.333-33','Permanente','Presencial',null,'Contratado'),
	(3,3,'Carlos','4444-0235','email7.com','Endereço 4','444.444.444-44','Permanente','Presencial',null,'Contratado');*/
INSERT INTO contasBancarias(codFuncionario,agenciaContaB,numeroContaB,tipoContaB)
VALUES 
	(1,'01','conta 1','corrente'),
	(2,'02','conta 2','conta salário'),
	(3,'03','conta 3','conta salario');

INSERT INTO projetos(codDepartamento,idCliente,nomeProjeto,descricaoProjeto,statusProjeto,valorProjeto,dataEntregaProjeto)
VALUES
	(3,1,'Projeto1',null,'Finalizado',100000,'2023-03-01'),
	(2,2,'Projeto2',null,'Em andamento',100000,'2024-02-25'),
	(3,3,'Projeto3',null,'Em andamento',100000,'2024-03-12');

INSERT INTO funcionariosProjeto(idProjeto,idFuncionario)
VALUES
	(2,1),
	(2,2),
	(3,2),
	(3,3);


select * from cargos
select * from clientes
select * from departamentos
select * from funcionarios
select * from contasBancarias
select * from projetos
select * from funcionariosProjeto











