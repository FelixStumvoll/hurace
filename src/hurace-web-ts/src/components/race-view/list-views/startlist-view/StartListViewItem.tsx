import React from 'react';
import styled from 'styled-components';
import { StartList } from '../../../../interfaces/StartList';
const StartNumber = styled.div`
    font-weight: bold;
`;

export const StartListViewItem: React.FC<{ startList: StartList }> = ({
    startList
}) => {
    return (
        <tr>
            <td>
                <StartNumber>{startList.startNumber}</StartNumber>
            </td>
            <td>{startList.skier.country.countryCode}</td>
            <td>
                {startList.skier.firstName} {startList.skier.lastName}
            </td>
        </tr>
    );
};
