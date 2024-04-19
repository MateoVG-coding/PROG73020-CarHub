import requests
import json

BASE_URL = "http://localhost:5001/api"  

def test_register_user():
    url = f"{BASE_URL}/register"
    headers = {'Content-Type': 'application/json'}
    data = {
        "UserName": "testuser",
        "Email": "test@example.com",
        "PhoneNumber": "1234567890",
        "FirstName": "John",
        "LastName": "Doe",
        "Password": "Admin_2003"
    }
    response = requests.post(url, headers=headers, data=json.dumps(data))
    
    assert response.status_code == 201, f"Failed to register user: {response.text}"
    print("User registered successfully.")

def test_login_user():
    url = f"{BASE_URL}/login"
    headers = {'Content-Type': 'application/json'}
    data = {
        "UserName": "testuser",
        "Password": "Admin_2003"
    }
    response = requests.post(url, headers=headers, data=json.dumps(data))
    
    assert response.status_code == 200, f"Failed to login user: {response.text}"
    print("User logged in successfully.")
    
def test_create_listing():
    url = f"{BASE_URL}/Listings"
    headers = {'Content-Type': 'application/json'}
    data = {
        "Value": 1000,
        "CarBrand": "Toyota",
        "CarModel": "T2000",
        "CarYear": 2000,
        "Description": "Test"
    }
    response = requests.post(url, headers=headers, data=json.dumps(data))
    
    assert response.status_code == 200, f"Failed to create listing: {response.text}"
    print("Listing created successfully.")

if __name__ == "__main__":
    test_register_user()
    test_login_user()
    test_create_listing()
    