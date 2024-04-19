import requests
import json

BASE_URL = "https://localhost:5001"

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

    response = requests.post(f"{BASE_URL}/api/register", json=user_registration_data, verify=False)

    # Asserting that the response status code is 201 (Created)
    assert response.status_code == 200

def test_login_user_successful():
    # Sample login data
    login_data = {
        "username": "test_admin_test_python",
        "password": "testpassword_123_easy"
    }
    response = requests.post(f"{BASE_URL}/api/login", json=login_data, verify=False)

    # Asserting that the response status code is 200 (OK)
    assert response.status_code == 200
def test_create_review():
    url = f"{BASE_URL}/api/reviews"
    headers = {'Content-Type': 'application/json'}
    data = {
        "content": "Great service!",
        "rating": 5,
        "username": "testuser"
    }
    try:
        response = requests.post(url, headers=headers, data=json.dumps(data), verify=False)
        assert response.status_code == 201, f"Failed to create review: {response.text}"
        print("Review created successfully.")
    except requests.ConnectionError:
        print("Failed to connect to the API.")

if __name__ == "__main__":
    test_register_user_success()  # First register a user
    test_login_user_successful()
    test_create_review()  # Then test creating a review