import requests

# Base URL of the API
base_url = "https://localhost:5001"

def test_login_user_successful():
    # Sample login data
    login_data = {
        "username": "test_admin_test_python",
        "password": "testpassword_123_easy"
    }
    response = requests.post(f"{base_url}/api/login", json=login_data, verify=False)

    # Asserting that the response status code is 200 (OK)
    assert response.status_code == 200

def test_login_user_unsuccessful():
    response = requests.post(f"{base_url}/api/login", json={}, verify=False)

    # Asserting that the response status code is 401 (Unauthorized)
    assert response.status_code == 401

def test_create_listing():
    # Sample listing data
    listing_data = {
        "CarBrand": "Toyota",
        "CarModel": "Corolla",
        "CarYear": 2020,
        "Value": 15000,
        "Description": "Low mileage, excellent condition"
    }
    response = requests.post(f"{base_url}/api/listings", json=listing_data, verify=False)

    # Asserting that the response status code is 201 (Created)
    assert response.status_code == 201

# Run the tests
test_login_user_successful()
# test_login_user_unsuccessful()
test_create_listing()
