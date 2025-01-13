﻿using GamaEdtech.Back.Domain.Base;

namespace GamaEdtech.Back.Domain.Cities;

public interface ICityRepository
{
	public Task<City?> GetBy(Id id);
	public Task<bool> ContainsCityWithNameInCountry(string name, Id countryId);
	public Task<bool> ContainsCityWithNameInState(string name, Id stateId);
	public Task Add(City city);
	public Task Remove(City city);
}
