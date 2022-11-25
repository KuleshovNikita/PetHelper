import { Box, Button, TextField, Typography } from "@mui/material";
import React, { useRef, useState } from "react";
import { useLocation } from "react-router";
import { UserLoginModel } from "../../models/User";
import { useNavigate, Link as RouterLink } from "react-router-dom";
import { useStore } from "../../api/stores/Store";
import { toast } from "react-toastify";

const EMAIL_REGEX = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/
type Focus = React.FocusEvent<HTMLInputElement | HTMLTextAreaElement, Element>; 

export function Login() {
    const navigate = useNavigate();
    const location = useLocation();

    const fromPage = location.state?.form?.pathname || "/";

    const [email, setEmail] = useState("");
    const [emailErrors, setEmailErrors] = useState("");
    const [password, setPassword] = useState("");
    const [passwordErrors, setPasswordErrors] = useState("");

    const { userStore } = useStore();

    const emailRef = useRef<HTMLInputElement>();
    const passwordRef = useRef<HTMLInputElement>();

    const handleEmailChange = (e: Focus) => {
        const email = e.target.value;

        if(email.length === 0) {
            setEmailErrors('Email is required');
        } else if(!email.match(EMAIL_REGEX)) {
            setEmailErrors('Email should be in the correct format');
        } else {
            setEmailErrors('');
        }

        setEmail(email);
    }

    const handlePasswordChange = (e: Focus) => {
        const password = e.target.value;

        if (password.length === 0) {
            setPasswordErrors('Please enter password');
        } else {
            setPasswordErrors('');
        }

        setPassword(password);
    }

    const hasErrors = () => {
        emailRef.current?.focus();
        passwordRef.current?.focus();
        passwordRef.current?.blur();

        const isTouched = email.length && password.length
        const hasAnyError = emailErrors.length || passwordErrors.length

        return !isTouched || (isTouched && hasAnyError)
    }

    const submit = async () => {
        if (hasErrors()) {
            return;
        }

        const user: UserLoginModel = { login: email, password: password };

        const result = await userStore.login(user);

        if(!result.isSuccessful) {
            toast.error(result.clientErrorMessage);
        } else {
            toast.success(`Successful login, ${userStore.user?.firstName}`);
            navigate(fromPage, { replace: true });
        }
    };

    const redirectToRegistration = () => {
        navigate("/register", { replace: true });
    }

    return (
        <>
            <Typography variant="h1" 
                        component="h1"
                        align="center">
                Pet Helper
            </Typography>
            <Box
                component="form"
                sx={{
                    "&": {
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                    },
                    "& .MuiTextField-root": { m: 1, width: "25ch" },
                    "& button": { my: 1 },
                    "& .MuiLink-root": { fontSize: "1.2rem" },
                }}
                noValidate
                autoComplete="off"
            >
                <TextField
                    label="Email"
                    variant="filled"
                    value={email}
                    required={true}
                    helperText= {emailErrors}
                    error={emailErrors.length !== 0}
                    inputRef={emailRef}
                    onFocus={(e) => handleEmailChange(e)}
                    onChange={handleEmailChange}
                />
                <TextField
                    label="Password"
                    variant="filled"
                    type="password"
                    value={password}
                    required={true}
                    helperText= {passwordErrors}
                    error={passwordErrors.length !== 0}
                    inputRef={passwordRef}
                    onFocus={(e) => handlePasswordChange(e)}
                    onChange={handlePasswordChange}
                />
                <Button variant="contained" size="large" onClick={submit}>
                    Login
                </Button>
                <Typography>
                    Or
                </Typography>
                <Button sx={{
                            "&": {
                                backgroundColor: "brown"
                            },
                            "&:hover": {
                                backgroundColor: "red"
                            }
                        }} 
                        variant="contained" 
                        size="large" 
                        onClick={redirectToRegistration}>
                    Register
                </Button>
            </Box>
        </>
    );
}
