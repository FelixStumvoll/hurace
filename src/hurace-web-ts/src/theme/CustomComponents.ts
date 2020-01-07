import styled from 'styled-components';
import { Link } from 'react-router-dom';

export const Card = styled.div`
    /* box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2); */
    border: 1px solid ${props => props.theme.gray};
    border-radius: 5px;
    padding: 10px;
`;

export const ListItem = styled.div`
    border-bottom: 1px solid ${props => props.theme.gray};
    display: flex;
    :last-child {
        border-bottom: none;
    }
`;

export const DefaultLink = styled(Link)`
    text-decoration: none;
    color: black;
`;

export const DefaultInput = styled.input`
    border-radius: 5px;
    padding-left: 10px;
    border: 1px solid ${props => props.theme.gray};
`;

export const FormFields = styled.div<{ rowCount: number }>`
    display: grid;
    grid-template-rows: repeat(auto ${props => props.rowCount});
    grid-template-columns: auto auto;
    gap: 10px;
`;
