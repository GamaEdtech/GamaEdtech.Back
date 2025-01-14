﻿using GamaEdtech.Back.DataSource.Utils;
using GamaEdtech.Back.Domain.Base;
using GamaEdtech.Back.Domain.Cities;
using Microsoft.EntityFrameworkCore;

namespace GamaEdtech.Back.DataSource.Cities;

public class SqlServerCityRepository : ICityRepository
{
	private readonly GamaEdtechDbContext _dbContext;

	public SqlServerCityRepository(GamaEdtechDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<City?> GetBy(Id id)
	{
		return await _dbContext.Cities.FindAsync(id);
	}

	public async Task<bool> ContainsCityWithNameInCountry(string name, Id countryId)
	{
		return await _dbContext.Cities
			.Where(x => x.Name == name && x.CountryId == countryId)
			.AnyAsync();
	}

	public async Task<bool> ContainsCityWithNameInState(string name, Id stateId)
	{
		return await _dbContext.Cities
			.Where(x => x.Name == name && x.StateId == stateId)
			.AnyAsync();
	}

	public async Task<bool> ContainsCityInCountryWith(Id id)
	{
		return await _dbContext.Cities
			.Where(x => x.CountryId == id)
			.AnyAsync();
	}

	public async Task Add(City city)
	{
		await _dbContext.Cities.AddAsync(city);
	}

	public async Task Remove(City city)
	{
		_dbContext.Cities.Remove(city);
	}
}
