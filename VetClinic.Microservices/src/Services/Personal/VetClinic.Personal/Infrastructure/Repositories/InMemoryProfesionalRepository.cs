namespace VetClinic.Personal.Infrastructure.Repositories;

using VetClinic.Personal.Domain.Entities;
using VetClinic.Personal.Domain.Interfaces;
using VetClinic.Personal.Domain.ValueObjects;

/// <summary>
/// Repositorio en memoria de Profesionales.
/// Precargado con el equipo completo de la clínica veterinaria.
/// </summary>
public class InMemoryProfesionalRepository : IProfesionalRepository
{
    private readonly List<Profesional> _profesionales;

    public InMemoryProfesionalRepository()
    {
        // Los mismos 7 profesionales del proyecto de consola original
        _profesionales = new List<Profesional>
        {
            new Profesional(Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Dra. Elena Martín", new Email("elena.martin@vetclinic.com"), "Veterinaria", new Especialidad("Cirugía")),
            new Profesional(Guid.Parse("22222222-2222-2222-2222-222222222222"),
                "Dr. Mateo Gómez", new Email("mateo.gomez@vetclinic.com"), "Veterinario", new Especialidad("Medicina General")),
            new Profesional(Guid.Parse("33333333-3333-3333-3333-333333333333"),
                "Dra. Sofía Ávila", new Email("sofia.avila@vetclinic.com"), "Veterinaria", new Especialidad("Urgencias")),
            new Profesional(Guid.Parse("44444444-4444-4444-4444-444444444444"),
                "Pedro Cortés", new Email("pedro.cortes@vetclinic.com"), "Peluquero", new Especialidad("Estética Canina")),
            new Profesional(Guid.Parse("55555555-5555-5555-5555-555555555555"),
                "Laura Ríos", new Email("laura.rios@vetclinic.com"), "Peluquera", new Especialidad("Estética Felina")),
            new Profesional(Guid.Parse("66666666-6666-6666-6666-666666666666"),
                "Lucía Rivera", new Email("lucia.rivera@vetclinic.com"), "Asistente", new Especialidad("Recepción")),
            new Profesional(Guid.Parse("77777777-7777-7777-7777-777777777777"),
                "Diego Pérez", new Email("diego.perez@vetclinic.com"), "Asistente", new Especialidad("Recepción")),
        };
    }

    public Task<Profesional?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => Task.FromResult(_profesionales.FirstOrDefault(p => p.Id == id));

    public Task<List<Profesional>> GetAllAsync(CancellationToken ct = default)
        => Task.FromResult(_profesionales.ToList());

    public Task AddAsync(Profesional profesional, CancellationToken ct = default)
    {
        _profesionales.Add(profesional);
        return Task.CompletedTask;
    }
}
