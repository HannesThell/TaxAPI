# Congestion Tax Calculator

Welcome the Volvo Cars Congestion Tax Calculator assignment.

This API provides the operation CalculateTax whch given a vehicleType and an array of dates returns the total congestion tax for the vehicle.

https://localhost:7050/api/tax/vehicle/calculateTax

It is a POST request with an example input as below.

{
"vehicleType": "car",
"taxationDates": ["2013-02-08T15:29:00", "2013-02-08T17:29:00"]
}

The operation is currently limited to the year 2013 and for the Swedish market.

Possible VehicleTypes are:

- Car
- Motorcycle
- Tractor
- Emergency
- Diplomat
- Foreign
- Military
