import requests

# Base URL of the API
base_url = "https://localhost:5001"

def test_register_user_success():
    # The test will fail if run with the same registration data
    user_registration_data = {
        "firstName": "Mateo",
        "lastName": "Vega",
        "userName": "test_admin_test_python",
        "password": "testpassword_123_easy",
        "email": "admin_test_python@example.com",
        "phoneNumber": "1234567890",
    }

    response = requests.post(f"{base_url}/api/register", json=user_registration_data, verify=False)

    # Asserting that the response status code is 201 (Created)
    assert response.status_code == 201

def test_register_user_failure():
    response = requests.post(f"{base_url}/api/register", json={}, verify=False)

    # Asserting that the response status code is 400 (Bad Request)
    assert response.status_code == 400

def test_login_user_successful():
    # Sample login data
    login_data = {
        "username": "testuser",
        "password": "testpassword"
    }
    response = requests.post(f"{base_url}/api/login", json=login_data, verify=False)

    # Asserting that the response status code is 200 (OK)
    assert response.status_code == 200

def test_login_user_unsuccessful():
    response = requests.post(f"{base_url}/api/login", json={}, verify=False)

    # Asserting that the response status code is 401 (Unauthorized)
    assert response.status_code == 401

# Run the tests
test_register_user_success()
test_register_user_failure()
test_login_user_successful()
test_login_user_unsuccessful()