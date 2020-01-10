import styled from 'styled-components';
import { Link } from 'react-router-dom';

export const Card = styled.div`
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
    border: 1px solid ${props => props.theme.gray};
    border-radius: 5px;
    padding: ${props => props.theme.gap};
    background-color: white;
`;

// export const ListItem = styled.div`
//     border-bottom: 1px solid ${props => props.theme.gray};
//     display: flex;
//     :last-child {
//         border-bottom: none;
//     }
// `;

export const ListItem = styled(Card)`
    padding: 10px ${props => props.theme.gap} 10px ${props => props.theme.gap};
    border-radius: 0;
    display: grid;
`;

export const DefaultLink = styled(Link)`
    text-decoration: none;
    color: black;
`;

export const DefaultButton = styled.button`
    border-radius: 5px;
    border: none;
    height: 30px;
    cursor: pointer;
    font-size: 16px;
    padding: 0 10px 0 10px;
    font-family: ${props => props.theme.fontFamily};
`;

export const DefaultInput = styled.input`
    border-radius: 5px;
    padding-left: 10px;
    height: 34px;
    border: 1px solid ${props => props.theme.gray};
`;

export const FormFields = styled.div<{ rowCount: number }>`
    display: grid;
    grid-template-rows: repeat(auto ${props => props.rowCount});
    grid-template-columns: auto auto;
    gap: 10px;
`;

export const NoListEntryText = styled.div`
    width: fit-content;
    margin: auto;
`;

export const RowFlex = styled.div`
    display: flex;
`;

export const ColumnFlex = styled.div`
    display: flex;
    flex-direction: column;
`;

export const FlexWrap = styled(RowFlex)`
    flex-wrap: wrap;
`;

export const AlignRight = styled.div`
    margin-left: auto;
`;

export const TextBold = styled.span`
    font-weight: bold;
`;

export const WrapText = styled.span`
    word-wrap: break-word;
    word-break: break-all;
`;

export const VerticallyAlignedText = styled.span`
    height: fit-content;
    margin: auto 0 auto 0;
`;
