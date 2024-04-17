$(document).ready(function () {
    var _newTaskItemMsg = $('#listingItemMsg');

    var _carList = $('#carList');
    var _carApiUrl = 'https://localhost:5001/api/Listings';

    var _reviewList = $('#reviewList');
    var _reviewApiUrl = 'https://localhost:5001/api/Reviews';

    var _loginUrl = 'https://localhost:5001/api/login';
    var _loginLink = $('#loginLink');
    var _loginModal = $('#loginModal').modal();

    var _registerUrl = 'https://localhost:5001/api/register';
    var _registerLink = $('#registerLink');
    var _registerModal = $('#registerModal').modal();

    // Function to handle form submission and apply filters
    var applyFilters = function () {
        // Get filter values from form inputs
        var filter = {
            SortByDate: $('#sortByDate').is(':checked'),
            Model: $('#model').val(),
            MinYear: $('#minYear').val(),
            MaxYear: $('#maxYear').val(),
            Brand: $('#brand').val()
        };

        // Make a GET request to the API with the filter parameters
        fetch(_carApiUrl + "/All?" + new URLSearchParams(filter), {
            method: 'GET',
            mode: 'cors'
        })
            .then((response) => {
                if (response.ok) {
                    return response.json(); // Parse response body as JSON
                } else {
                    return Promise.reject(response);
                }
            })
            .then((cars) => {
                _carList.empty();

                if (cars.length === 0) {
                    _carList.append('<li>No cars available.</li>');
                } else {
                    for (let i = 0; i < cars.length; i++) {
                        _carList.append('<li>' + cars[i].CarBrand + ' ' + cars[i].CarModel + ' - Year: ' +
                            cars[i].CarYear + ', Price: ' + cars[i].Value + '</li>');
                    }
                }
            })
            .catch((error) => {
                console.log('Error applying filters:', error);
            });
    };

    var setLoginState = function (isLoggedIn) {
        _isUserLoggedIn = isLoggedIn;
        if (isLoggedIn) {
            _loginLink.text('Logout');
        } else {
            _loginLink.text('Login');
        }
    };

    var loadReviewList = function (username) {
        const loadReviewsPromise = fetch(_reviewApiUrl + '?username=' + username, {
            mode: 'cors',
            headers: {
                'Accept': 'application/json'
            }
        });

        loadReviewsPromise.then((response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                return Promise.reject(response);
            }
        })
            .then((reviews) => {
                _reviewList.empty();

                if (reviews.length === 0) {
                    _reviewList.append('<li>No reviews available.</li>');
                } else {
                    for (let i = 0; i < reviews.length; i++) {
                        _reviewList.append('<li>' + reviews[i].content + ' - Rating: ' +
                            reviews[i].rating + '</li>');
                    }
                }
            })
            .catch((response) => {
                console.log(`fetch review list; resp code: ${response.status}`);
            });
    };
    var loadApiHome = function () {
        fetch(_carApiUrl, {
            method: 'GET',
            mode: 'cors',
            headers: {
                'Accept': 'application/json'
            }
        })
            .then(response => {
                if (response.ok) {
                    return response.json();
                } else {
                    throw new Error('Failed to fetch API home');
                }
            })
            .then(data => {
                console.log(data); 
            })
            .catch(error => {
                console.error('Error fetching API home:', error);
            });
    };
    var loadCarList = function () {
        const loadCarsPromise = fetch(_carApiUrl + "/All", {
            mode: 'cors',
            headers: {
                'Accept': 'application/json'
            }
        });

        loadCarsPromise.then((response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                return Promise.reject(response);
            }
        })
            .then((cars) => {
                _carList.empty();

                if (cars.length === 0) {
                    _carList.append('<li>No cars available.</li>');
                } else {
                    for (let i = 0; i < cars.length; i++) {
                        _carList.append('<li>' + cars[i].CarBrand + ' ' + cars[i].CarModel + ' - Year: ' +
                            cars[i].CarYear + ', Price: ' + cars[i].Value + '</li>');
                    }
                }
            })
            .catch((response) => {
                console.log(`fetch car list; resp code: ${response.status}`);
            });
    };

    loadApiHome();
    loadCarList();
    loadReviewList();

    $('#createListingBtn').click(function () {
        let newListing = {
            CarBrand: $('#make').val(),
            CarModel: $('#model').val(),
            CarYear: $('#year').val(),
            Value: $('#price').val(),
            Description: $('#description').val()
        };

        const createListingPromise = fetch(_carApiUrl, {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newListing)
        });

        createListingPromise.then((response) => {
            if (response.status === 201) {
                console.log('Listing created successfully.');
                loadCarList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`create listing; resp code: ${response.status}`);
            });
    });

    $('#deleteListingBtn').click(function () {
        let listingId = $('#listingIdToDelete').val();

        const deleteListingPromise = fetch(`${_carApiUrl}/${listingId}`, {
            method: 'DELETE',
            mode: 'cors'
        });

        deleteListingPromise.then((response) => {
            if (response.status === 204) {
                console.log('Listing deleted successfully.');
                loadCarList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`delete listing; resp code: ${response.status}`);
            });
    });

    $('#updateListingBtn').click(function () {
        let listingId = $('#listingIdToUpdate').val();
        let updatedListing = {
            make: $('#updatedMake').val(),
            model: $('#updatedModel').val(),
            year: $('#updatedYear').val(),
            price: $('#updatedPrice').val()
        };

        const updateListingPromise = fetch(`${_carApiUrl}/${listingId}`, {
            method: 'PUT',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedListing)
        });

        updateListingPromise.then((response) => {
            if (response.status === 204) {
                console.log('Listing updated successfully.');
                loadCarList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`update listing; resp code: ${response.status}`);
            });
    });

    $('#createReviewBtn').click(function () {
        let newReview = {
            content: $('#content').val(),
            rating: $('#rating').val()
        };

        const createReviewPromise = fetch(_reviewApiUrl, {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(newReview)
        });

        createReviewPromise.then((response) => {
            if (response.status === 201) {
                console.log('Review created successfully.');
                loadReviewList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`create review; resp code: ${response.status}`);
            });
    });

    $('#deleteReviewBtn').click(function () {
        let reviewId = $('#reviewIdToDelete').val();

        const deleteReviewPromise = fetch(`${_reviewApiUrl}/${reviewId}`, {
            method: 'DELETE',
            mode: 'cors'
        });

        deleteReviewPromise.then((response) => {
            if (response.status === 204) {
                console.log('Review deleted successfully.');
                loadReviewList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`delete review; resp code: ${response.status}`);
            });
    });

    $('#updateReviewBtn').click(function () {
        let reviewId = $('#reviewIdToUpdate').val();
        let updatedReview = {
            content: $('#updatedContent').val(),
            rating: $('#updatedRating').val()
        };

        const updateReviewPromise = fetch(`${_reviewApiUrl}/${reviewId}`, {
            method: 'PUT',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updatedReview)
        });

        updateReviewPromise.then((response) => {
            if (response.status === 204) {
                console.log('Review updated successfully.');
                loadReviewList();
            } else {
                return Promise.reject(response);
            }
        })
            .catch((response) => {
                console.log(`update review; resp code: ${response.status}`);
            });
    });

    // Event listener for form submission
    $('#filterForm').submit(function (event) {
        // Prevent default form submission behavior
        event.preventDefault();

        // Apply filters when the form is submitted
        applyFilters();
    });

    // Event listener for applying filters using the button
    $('#applyFiltersBtn').click(function () {
        // Apply filters when the button is clicked
        applyFilters();
    });

    $('#loginLink').click(function () {
        _loginModal.modal('show');
    });

    $('#loginBtn').click(function () {
        let loginRequest = {
            userName: $('#username').val(),
            password: $('#password').val()
        };

        const loginPromise = fetch(_loginUrl, {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(loginRequest)
        });

        loginPromise.then((response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                return Promise.reject(response);
            }
        })
            .then((tokenInfo) => {
                _currentAccessToken = tokenInfo.token;

                _newTaskItemMsg.attr('class', 'text-success');
                _newTaskItemMsg.text('You are logged in');
                setLoginState(true);
                $('#password').val('');

                run();
                _newTaskItemMsg.fadeOut(10000);
            })
            .catch((response) => {
                console.log(`fetch API home page; resp code: ${response.status}`);

                _newTaskItemMsg.attr('class', 'text-danger');
                _newTaskItemMsg.text('Hmmm, there was a problem logging you in.');
                _newTaskItemMsg.fadeOut(10000);
            });
    });

    $('#registerLink').click(function () {
        _registerModal.modal('show');
    });

    $('#registerBtn').click(function () {
        let registerRequest = {
            userName: $('#registerUsername').val(),
            password: $('#registerPassword').val(),
            email: $('#registerEmail').val(),
            firstName: $('#registerFirstName').val(),
            lastName: $('#registerLastName').val(),
            phoneNumber: $('#phoneNumber').val()
        };

        const registerPromise = fetch(_registerUrl, {
            method: 'POST',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(registerRequest)
        });

        registerPromise.then((response) => {
            if (response.status === 200) {
                return response.json();
            } else {
                return Promise.reject(response);
            }
        })
            .then((tokenInfo) => {
                _currentAccessToken = tokenInfo.token;

                _newTaskItemMsg.attr('class', 'text-success');
                _newTaskItemMsg.text('You are registered');
                setLoginState(true);
                $('#password').val('');

                run();
                _newTaskItemMsg.fadeOut(10000);
            })
            .catch((response) => {
                console.log(`fetch API home page; resp code: ${response.status}`);

                _newTaskItemMsg.attr('class', 'text-danger');
                _newTaskItemMsg.text('Hmmm, there was a problem logging you in.');
                _newTaskItemMsg.fadeOut(10000);
            });
    });
});


