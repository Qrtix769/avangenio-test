﻿using System.ComponentModel.DataAnnotations;

namespace Api.CQRS.ContactRequests.GetAllContacts;

public class GetAllContactsDto
{
	public Guid Id { get; set; }

	public string FirstName { get; set; } = null!;

	public string? LastName { get; set; }

	public string Email { get; set; } = null!;

	public DateTime DateOfBirth { get; set; }

	public int Age { get; set; }

	public string Phone { get; set; } = null!;

	public Guid Owner { get; set; }
}