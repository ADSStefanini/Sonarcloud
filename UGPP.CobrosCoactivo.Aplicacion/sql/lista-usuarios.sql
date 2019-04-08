-- SELECT * FROM usuarios
-- SELECT * FROM perfiles
-----------------------------
SELECT usuarios.codigo, usuarios.nombre, usuarios.nivelacces, 
	usuarios.login, usuarios.useractivo, usuarios.nivelacces,
	perfiles.nombre as nomperfil 
	FROM usuarios 
		LEFT JOIN perfiles ON usuarios.nivelacces = perfiles.codigo
	ORDER BY usuarios.nivelacces	