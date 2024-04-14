import requests
import json

BASE_URL = "http://localhost:5000/api"  

def test_register_user():
    url = f"{BASE_URL}/register"
    headers = {'Content-Type': 'application/json'}
    data = {
        "UserName": "testuser",
        "Email": "test@example.com",
        "PhoneNumber": "1234567890",
        "FirstName": "John",
        "LastName": "Doe",
        "Password": "password",
        "Roles": ["User"]
    }
    response = requests.post(url, headers=headers, data=json.dumps(data))
    
    assert response.status_code == 201, f"Failed to register user: {response.text}"
    print("User registered successfully.")

def test_login_user():
    url = f"{BASE_URL}/login"
    headers = {'Content-Type': 'application/json'}
    data = {
        "UserName": "testuser",
        "Password": "password"
    }
    response = requests.post(url, headers=headers, data=json.dumps(data))
    
    assert response.status_code == 200, f"Failed to login user: {response.text}"
    print("User logged in successfully.")

if __name__ == "__main__":
    test_register_user()
    test_login_user()