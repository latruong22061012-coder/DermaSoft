<?php

declare(strict_types=1);

class AuthController extends Controller
{
    public function login(): void
    {
        $this->render('login');
    }

    public function register(): void
    {
        $this->render('register');
    }

    public function forgotPassword(): void
    {
        $this->render('forgot-password');
    }
}
