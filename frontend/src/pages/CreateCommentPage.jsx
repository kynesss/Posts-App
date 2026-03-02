import { Button, Box, TextField, Stack, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import { useParams, useNavigate } from "react-router-dom";
import { useState } from "react";
import useCommentsMutations from "../hooks/useCommentsMutations";

const CreateCommentPage = () => {
  const { postId } = useParams();
  const { createComment } = useCommentsMutations();
  const [content, setContent] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!content.trim()) return;
    try {
      await createComment(postId, { content });
      navigate(`/posts/${postId}`);
    } catch {
      //
    }
  };
  return (
    <Box
      sx={{ maxWidth: 900, m: "auto" }}
      component="form"
      onSubmit={handleSubmit}
    >
      <Stack spacing={5}>
        <Button
          variant="contained"
          component={Link}
          to={`/posts/${postId}`}
          sx={{ maxWidth: 150 }}
        >
          Wróć
        </Button>
        <Typography variant="h4">Dodaj komentarz</Typography>
        <TextField
          label="Treść"
          variant="outlined"
          value={content}
          onChange={(e) => setContent(e.target.value)}
          minRows={5}
          multiline
          required
        ></TextField>
        <Button
          variant="contained"
          sx={{ maxWidth: 150, m: "auto" }}
          type="submit"
        >
          Wyślij
        </Button>
      </Stack>
    </Box>
  );
};

export default CreateCommentPage;
