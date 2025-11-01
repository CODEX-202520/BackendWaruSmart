-- Script para crear un usuario administrador de prueba
-- Ejecutar este script después de crear la base de datos y aplicar las migraciones

-- NOTA: Asegúrate de que ya existe un usuario en la tabla 'users'
-- Este script actualiza el rol de un usuario existente a ADMINISTRADOR_WARU_SMART

-- Opción 1: Actualizar el primer usuario registrado para que sea administrador
UPDATE users 
SET role = 'ADMINISTRADOR_WARU_SMART' 
WHERE id = (SELECT MIN(id) FROM users);

-- Opción 2: Actualizar un usuario específico por username
-- Reemplaza 'tu_usuario' con el nombre de usuario que desees hacer administrador
UPDATE users 
SET role = 'ADMINISTRADOR_WARU_SMART' 
WHERE username = 'tu_usuario';

-- Opción 3: Ver todos los usuarios y sus roles actuales
SELECT id, username, role 
FROM users 
ORDER BY id;

-- Opción 4: Crear suscripciones de ejemplo (solo si ya tienes un admin)
-- Estas suscripciones de ejemplo pueden ser útiles para probar el sistema

INSERT INTO subscriptions (name, description, price, duration_in_days, is_active) 
VALUES 
('Plan Básico', 'Perfecto para comenzar con WaruSmart', 9.99, 30, 1),
('Plan Profesional', 'Todas las funcionalidades para profesionales', 29.99, 30, 1),
('Plan Empresarial', 'Solución completa para empresas agrícolas', 99.99, 90, 1);

-- Verificar las suscripciones creadas
SELECT * FROM subscriptions;

-- IMPORTANTE: 
-- 1. El password_hash debe ser generado por la aplicación al registrar un usuario
-- 2. No intentes insertar usuarios directamente con contraseñas en texto plano
-- 3. Usa el endpoint de registro (sign-up) para crear nuevos usuarios
-- 4. Luego ejecuta uno de los UPDATE de arriba para hacerlo administrador
