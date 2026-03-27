<?php

declare(strict_types=1);

use App\Core\Auth;

class HomeController extends Controller
{
    public function index(): void
    {
        Auth::startSession();
        $isLoggedIn = Auth::isAuthenticated();
        $currentUser = $isLoggedIn ? Auth::getCurrentUser() : null;
        $this->render('home', ['isLoggedIn' => $isLoggedIn, 'currentUser' => $currentUser]);
    }
}
