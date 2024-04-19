import requests
import json

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
    

def test_create_review():
    url = f"{base_url}/api/reviews"
    headers = {'Content-Type': 'application/json'}
    data = {
        "content": "Great service!",
        "rating": 5,
        "username": "test"
    }
    response = requests.post(f"{base_url}/api/reviews", json=data, verify=False)

    # Asserting that the response status code is 201 (Created)
    assert response.status_code == 201

if __name__ == "__main__":
    test_login_user_successful()
    test_create_review()  # Then test creating a review