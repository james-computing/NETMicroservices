Este é um projeto ASP.NET Core que usa a arquitetura de microserviços. Ele é uma adaptação do tutorial abaixo, mas com diversas modificações para utilizar versões mais recentes das tecnologias.

[![Video Title](https://img.youtube.com/vi/DgVjEo3OGBI/0.jpg)](https://www.youtube.com/watch?v=DgVjEo3OGBI)

As tecnologias utilizadas são:
<ul>
  <li> ASP.NET Core </li>
  <li> Banco de dados SQLServer </li>
  <li> Docker </li>
  <li> Kubernetes </li>
  <li> RabbitMQ </li>
  <li> gRPC </li>
</ul>

# Estrutura do Projeto

A arquitetura do projeto se resume no diagrama abaixo, em que somente as componentes principais são ilustradas:

<p align="center">
<img height="500" alt="design" src="https://github.com/user-attachments/assets/a8b57051-4019-4132-af02-0f490a39251f" />
</p>

O projeto consiste em 2 serviços principais, que são aplicações ASP.NET Core. Um é o Platforms service e o outro é o Commands service. Essas duas aplicações implementam APIs REST e executam em pods de Kubernetes. As APIs desses serviços são simples, sendo o objetivo maior demonstrar como montar uma arquitetura de microserviços. Plataformas são criadas no Platforms service e elas são também utilizadas pelo Commands service. Sendo dois serviços distintos, é necessária alguma forma de comunicação entre eles, para que o Commands service tenha acesso às plataformas do Platforms service. São utilizadas duas formas de comunição, uma assíncrona que utiliza o RabbitMQ, e outra que utiliza gRPC. Também um método POST é utilizado para testar a comunicação entre o Platforms service e o Commands service, mas ele não é utilizado para enviar plataformas para o Commands service.

# Algumas modificações do tutorial

Dia 12 de Novembro de 2025 foi anunciada a aposentadoria do projeto Ingress NGINX. Esse serviço era utilizado pelo tutorial para receber requisições do cliente e enviar para os serviços em Kubernetes. Devido a isso, foi necessário adaptar o projeto. Seguindo a recomendação oficial, substituí o Ingress NGINX por uma implementação do Gateway API. Utilizei a implementação da NGINX, que é a NGINX Gateway Fabric.

Além disso, no tutorial é utilizado o RabbitMQ sem o RabbitMQ Cluster Operator. Seguindo a recomendação oficial do RabbitMQ, utilizei o RabbitMQ Cluster Operator para gerar um cluster do RabbitMQ. Também a API do RabbitMQ passou a utilizar métodos async em C#. Como não é possível utilizar métodos async num construtor, a conexão da aplicação com o RabbitMQ foi feita executando um método async no Program.cs.
