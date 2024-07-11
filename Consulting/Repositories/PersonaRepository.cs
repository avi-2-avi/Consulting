using System.Data.SqlClient;
using Consulting.Models;


namespace Consulting.Repositories;

public class PersonaRepository
{
     private readonly string _connectionString;

    public PersonaRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<IEnumerable<Persona>> GetPersonasAsync()
    {
        var personas = new List<Persona>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Personas", conn);
            conn.Open();
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    personas.Add(new Persona
                    {
                        Id = reader.GetInt32(0),
                        Numero = reader.GetInt32(1),
                        FechaHora = reader.GetDateTime(2),
                        NombreApellido = reader.GetString(3)
                    });
                }
            }
        }

        return personas;
    }

    public async Task<Persona> GetPersonaByIdAsync(int id)
    {
        Persona persona = null;

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Personas WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
            {
                if (reader.Read())
                {
                    persona = new Persona
                    {
                        Id = reader.GetInt32(0),
                        Numero = reader.GetInt32(1),
                        FechaHora = reader.GetDateTime(2),
                        NombreApellido = reader.GetString(3)
                    };
                }
            }
        }

        return persona;
    }

    public async Task AddPersonaAsync(Persona persona)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Personas (Numero, FechaHora, NombreApellido) VALUES (@Numero, @FechaHora, @NombreApellido)", conn);
            cmd.Parameters.AddWithValue("@Numero", persona.Numero);
            cmd.Parameters.AddWithValue("@FechaHora", persona.FechaHora);
            cmd.Parameters.AddWithValue("@NombreApellido", persona.NombreApellido);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task UpdatePersonaAsync(Persona persona)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("UPDATE Personas SET Numero = @Numero, FechaHora = @FechaHora, NombreApellido = @NombreApellido WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", persona.Id);
            cmd.Parameters.AddWithValue("@Numero", persona.Numero);
            cmd.Parameters.AddWithValue("@FechaHora", persona.FechaHora);
            cmd.Parameters.AddWithValue("@NombreApellido", persona.NombreApellido);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task DeletePersonaAsync(int id)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Personas WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}