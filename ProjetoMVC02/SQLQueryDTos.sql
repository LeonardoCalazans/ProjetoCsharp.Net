select 
Categoria, 
count(*) Quantidade
from Compromisso
where IdUsuario = 1
group by Categoria