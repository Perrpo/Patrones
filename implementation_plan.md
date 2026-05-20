# Arquitectura Empresarial ArchiMate — Clínica Veterinaria

## Objetivo
Crear una visualización interactiva de la **Arquitectura Empresarial** de la Clínica Veterinaria usando notación **ArchiMate**, implementada como una página web HTML interactiva con las 4 capas requeridas, relaciones horizontales y verticales, y análisis de impacto.

> [!IMPORTANT]
> Se implementará como una **página web HTML interactiva** para facilitar la presentación en video. Incluirá colores ArchiMate estándar, tooltips, y análisis de impacto interactivo al hacer clic en componentes.

---

## Capas y Componentes Diseñados

### 🟣 Capa 1: Motivación / Estrategia (14 componentes — violeta)

| # | Tipo ArchiMate | Nombre | Descripción |
|---|---|---|---|
| 1 | **Stakeholder** | Dueño/Gerente de la Clínica | Principal interesado, toma decisiones estratégicas |
| 2 | **Stakeholder** | Clientes (Dueños de Mascotas) | Usuarios finales del servicio veterinario |
| 3 | **Stakeholder** | Veterinarios y Personal | Profesionales que prestan los servicios |
| 4 | **Driver** | Crecimiento de la demanda de servicios veterinarios | Tendencia del mercado pet-care |
| 5 | **Driver** | Necesidad de digitalización operativa | Eliminar procesos manuales y papel |
| 6 | **Driver** | Competencia en el sector veterinario | Diferenciación por calidad de servicio |
| 7 | **Assessment** | Procesos manuales generan errores y demoras | Evaluación del estado actual |
| 8 | **Assessment** | Baja trazabilidad de historiales clínicos | Riesgo operativo identificado |
| 9 | **Goal** | Mejorar la experiencia del cliente | Objetivo estratégico principal |
| 10 | **Goal** | Optimizar la gestión operativa de la clínica | Eficiencia interna |
| 11 | **Goal** | Garantizar disponibilidad 24/7 del sistema | Alta disponibilidad |
| 12 | **Outcome** | Reducción del 40% en tiempos de agendamiento | Resultado medible esperado |
| 13 | **Requirement** | Sistema de agendamiento en línea con validación de conflictos | Requerimiento funcional clave |
| 14 | **Requirement** | Sistema de facturación con múltiples métodos de pago | Requerimiento funcional |

---

### 🟡 Capa 2: Negocio (18 componentes — amarillo)

| # | Tipo ArchiMate | Nombre | Descripción |
|---|---|---|---|
| 1 | **Actor** | Cliente (Dueño de Mascota) | Persona que solicita servicios veterinarios |
| 2 | **Actor** | Veterinario | Profesional que atiende las citas |
| 3 | **Actor** | Peluquero Canino | Profesional de estética animal |
| 4 | **Actor** | Asistente Administrativo | Personal de apoyo operativo |
| 5 | **Role** | Profesional de Atención | Rol que agrupa a Vet, Peluquero, Asistente |
| 6 | **Role** | Administrador del Sistema | Gestiona configuración y reportes |
| 7 | **Business Process** | Proceso de Agendamiento de Citas | Dominio seleccionado DDD — proceso completo |
| 8 | **Business Process** | Proceso de Facturación y Pagos | Generación y cobro de facturas |
| 9 | **Business Process** | Proceso de Gestión de Personal | Alta/baja de profesionales |
| 10 | **Business Process** | Proceso de Notificaciones | Envío de confirmaciones y recordatorios |
| 11 | **Business Service** | Servicio de Atención Veterinaria Integral | Servicio de negocio de nivel 4 (SOA) |
| 12 | **Business Service** | Servicio de Gestión de Clientes y Mascotas | Catálogo de servicios |
| 13 | **Business Function** | Gestión de Agenda y Disponibilidad | Función de negocio core |
| 14 | **Business Function** | Gestión del Catálogo de Servicios | Consulta, Vacunación, Cirugía, etc. |
| 15 | **Business Object / Document** | Historia Clínica de Mascota | Documento del negocio |
| 16 | **Business Object / Document** | Factura de Servicios | Documento financiero |
| 17 | **Business Object / Document** | Reporte de Citas y Pagos | Reporte operativo |
| 18 | **Contract** | Contrato de Prestación de Servicios | Términos de servicio con clientes |

---

### 🔵 Capa 3: Aplicaciones / Sistemas (16 componentes — azul)

| # | Tipo ArchiMate | Nombre | Descripción |
|---|---|---|---|
| 1 | **Application Component** | Microservicio de Agendamiento | Bounded Context principal (puerto 5001) |
| 2 | **Application Component** | Microservicio de Facturación | Bounded Context de pagos (puerto 5002) |
| 3 | **Application Component** | Microservicio de Notificaciones | Bounded Context de alertas |
| 4 | **Application Component** | Microservicio de Personal | Bounded Context de gestión (puerto 5003) |
| 5 | **Application Component** | API Gateway | Punto de entrada unificado |
| 6 | **Application Component** | BuildingBlocks (Shared Kernel) | Clases base DDD compartidas |
| 7 | **Application Service** | Servicio de Agendamiento (REST API) | /api/agendamiento/* |
| 8 | **Application Service** | Servicio de Facturación (REST API) | /api/facturacion/* |
| 9 | **Application Service** | Servicio de Notificaciones (REST API) | /api/notificaciones/* |
| 10 | **Application Interface** | REST API Gateway Interface | Interfaz HTTP unificada |
| 11 | **Data Object** | Dataset de Citas y Agendas | Datos del BC Agendamiento |
| 12 | **Data Object** | Dataset de Facturas y Pagos | Datos del BC Facturación |
| 13 | **Data Object** | Dataset de Notificaciones | Datos del BC Notificaciones |
| 14 | **Data Object** | Dataset de Clientes y Mascotas | Datos del BC Personal |
| 15 | **Data Object** | Dataset del Catálogo de Servicios | Precios, duraciones, especialidades |
| 16 | **Application Event** | Bus de Eventos de Dominio | CitaCreadaEvent, CitaConfirmadaEvent, etc. |

---

### 🟢 Capa 4: Infraestructura / Tecnología (14 componentes — verde)

| # | Tipo ArchiMate | Nombre | Descripción |
|---|---|---|---|
| 1 | **Node** | Cluster Kubernetes (Azure AKS) | Orquestador de contenedores en nube |
| 2 | **Node** | Nodo de Aplicación 1 | Pod para Agendamiento + Facturación |
| 3 | **Node** | Nodo de Aplicación 2 | Pod para Notificaciones + Personal |
| 4 | **Node** | Nodo del API Gateway | Pod del Gateway + Load Balancer |
| 5 | **Device** | Azure SQL Server (Primary) | Base de datos principal |
| 6 | **Device** | Azure SQL Server (Replica) | Base de datos réplica para lectura |
| 7 | **System Software** | .NET 8 Runtime | Entorno de ejecución |
| 8 | **System Software** | SQL Server 2022 | Motor de base de datos |
| 9 | **System Software** | RabbitMQ | Message broker para eventos |
| 10 | **Artifact** | Docker Container Images | Imágenes de los 4 microservicios |
| 11 | **Communication Network** | Azure Virtual Network (VNet) | Red privada virtual |
| 12 | **Communication Network** | Internet / CDN | Red pública + distribución |
| 13 | **Facility** | Azure Region — East US | Centro de datos en nube |
| 14 | **Infrastructure Service** | Azure Monitor + App Insights | Monitoreo y telemetría |

---

## Relaciones entre Capas

### Verticales (entre capas)
- **Motivación → Negocio**: Los Goals y Requirements motivan/realizan los Business Processes y Services
- **Negocio → Aplicaciones**: Los Business Processes son soportados por los Application Components y Services
- **Aplicaciones → Infraestructura**: Los Application Components se despliegan en Nodes y usan Data Objects almacenados en Devices

### Horizontales (dentro de cada capa)
- **Motivación**: Stakeholders asociados a Drivers, Drivers influencian Assessments, Assessments conducen a Goals, Goals se refinan en Requirements
- **Negocio**: Actors asignados a Roles, Roles participan en Processes, Processes usan Functions, Functions generan Documents
- **Aplicaciones**: Microservicios exponen Services a través del Gateway, Services acceden a Data Objects, Events comunican entre componentes
- **Infraestructura**: Nodes dentro del Cluster, Devices conectados por Network, todo dentro del Facility

---

## Análisis de Impacto (3 escenarios)

### Escenario 1: "¿Qué pasa si cambia el proceso de agendamiento?"
- **Impacto Motivación**: Afecta Goal "Mejorar experiencia del cliente", Requirement "Sistema de agendamiento"
- **Impacto Negocio**: Modifica Process "Agendamiento de Citas", afecta Documents "Historia Clínica"
- **Impacto Aplicaciones**: Requiere cambios en Microservicio Agendamiento, Dataset Citas, Bus de Eventos
- **Impacto Infraestructura**: Posible redeploy en Nodo App 1, actualización de Container Image

### Escenario 2: "¿Qué pasa si se agrega un nuevo método de pago?"
- **Impacto Motivación**: Soporta Goal "Mejorar experiencia del cliente"
- **Impacto Negocio**: Extiende Process "Facturación y Pagos", actualiza Document "Factura"
- **Impacto Aplicaciones**: Cambio en Microservicio Facturación (nueva Strategy), Dataset Facturas
- **Impacto Infraestructura**: Redeploy Nodo App 1, posible integración con pasarela de pago externa

### Escenario 3: "¿Qué pasa si cae el servidor de base de datos principal?"
- **Impacto Infraestructura**: Failover a Azure SQL Replica
- **Impacto Aplicaciones**: Todos los Datasets entran en modo lectura temporal
- **Impacto Negocio**: Todos los Processes degradados, Documents no se actualizan
- **Impacto Motivación**: Afecta Goal "Disponibilidad 24/7", Assessment de riesgos operativos

---

## Implementación Técnica

### [NEW] archimate_enterprise.html
Página web interactiva con:
- **Visualización por capas** con colores ArchiMate estándar (violeta, amarillo, azul, verde)
- **Componentes clickeables** con tooltips descriptivos
- **Líneas de relación** horizontales y verticales con etiquetas ArchiMate
- **Análisis de impacto interactivo**: al seleccionar un componente, se resaltan todos los componentes afectados
- **Diseño responsive** y moderno para presentación en video
- **Panel lateral** con detalles del componente seleccionado y cadena de impacto

---

## Verificación

### Visual
- Verificar que los 4 niveles sean visibles y distinguibles por color
- Verificar que los componentes mínimos se cumplan (12+15+16+14 = 57 total)
- Verificar relaciones horizontales y verticales

### Funcional
- Probar análisis de impacto clickeando componentes
- Verificar que la cadena de impacto sea coherente entre capas
