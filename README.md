# coordinates-distance-calculator

Current functionality:

The API exposes 3 endpoints for calculating the distance between 2 points:
* distance/sphere    -> uses spherical formula
* distance/flat      -> uses Pythagora's formula 
* distance/ellipsoid -> uses GeoCoordinatePortable nuGet package for the calculus

The endpoints use different formulas for calculating the distance.

The user can specify the unit system in which to measure the distance (Metric / Imperial) and the distance will be updated accordingly to return in km/miles.
If the unit is not specified, the API will attempt to get the user's ip address, will make a request to a third party API to get the country based on IP and will identify the unit that needs to be used.

The endpoints enforce validation on the coordinates so the latitude is between -90 and 90 and the longitude between -180 and 180.

Has unit tests and integration tests.

